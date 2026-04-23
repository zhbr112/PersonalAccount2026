-- Скрипт миграции
-- 2026-04-16
-- Перенос настроек загрузки на уровень филиалов и перепривязка данных.

create table if not exists branches
(
    id uuid not null primary key default gen_random_uuid(),
    company_id uuid not null,
    name text not null,
    load_options jsonb,
    is_default boolean not null default false
);

alter table branches
add constraint branches_company_id_fk
foreign key (company_id)
references companies(id);

create index if not exists ix_branches_company_id on branches(company_id);
create unique index if not exists ux_branches_company_default on branches(company_id) where is_default = true;

insert into branches(company_id, name, load_options, is_default)
select c.id,
       concat('Основной филиал ', coalesce(c.name, c.inn, c.id::text)),
       c.load_options,
       true
from companies c
where not exists (
    select 1
    from branches b
    where b.company_id = c.id
      and b.is_default = true
);

alter table transactions add column if not exists branch_id uuid;
alter table journal add column if not exists branch_id uuid;
alter table categories add column if not exists branch_id uuid;
alter table emploees add column if not exists branch_id uuid;
alter table links_user_company add column if not exists branch_id uuid;

update transactions t
set branch_id = b.id
from branches b
where b.company_id = t.company_id
  and b.is_default = true
  and t.branch_id is null;

update journal j
set branch_id = b.id
from branches b
where b.company_id = j.company_id
  and b.is_default = true
  and j.branch_id is null;

update categories c
set branch_id = b.id
from branches b
where b.company_id = c.company_id
  and b.is_default = true
  and c.branch_id is null;

update emploees e
set branch_id = b.id
from branches b
where b.company_id = e.company_id
  and b.is_default = true
  and e.branch_id is null;

update links_user_company l
set branch_id = b.id
from branches b
where b.company_id = l.company_id
  and b.is_default = true
  and l.branch_id is null;

alter table transactions alter column branch_id set not null;
alter table journal alter column branch_id set not null;

alter table transactions
add constraint transactions_branch_id_fk
foreign key (branch_id)
references branches(id);

alter table journal
add constraint journal_branch_id_fk
foreign key (branch_id)
references branches(id);

alter table categories
add constraint categories_branch_id_fk
foreign key (branch_id)
references branches(id);

alter table emploees
add constraint emploees_branch_id_fk
foreign key (branch_id)
references branches(id);

alter table links_user_company
add constraint links_user_company_branch_id_fk
foreign key (branch_id)
references branches(id);

create index if not exists ix_transactions_branch_id on transactions(branch_id);
create index if not exists ix_journal_branch_id on journal(branch_id);
create index if not exists ix_journal_company_branch on journal(company_id, branch_id);

alter table transactions drop constraint if exists transactions_company_id_fk;
drop index if exists ix_transactions_company_id;
alter table transactions drop column if exists company_id;

alter table categories drop constraint if exists categories_company_id_fk;
alter table categories drop column if exists company_id;

alter table emploees drop constraint if exists emploees_company_id_fk;
alter table emploees drop column if exists company_id;

alter table links_user_company drop constraint if exists links_user_company_company_id_fk;
alter table links_user_company drop column if exists company_id;

alter table companies drop column if exists load_options;