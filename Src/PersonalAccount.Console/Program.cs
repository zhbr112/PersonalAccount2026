using Microsoft.Extensions.Configuration;
using PersonalAccount.Console.Models;
using PersonalAccount.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using PersonalAccount.Domain.Models.Dto;

CurrentApplication.ShowLogo();

var builder  = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();
var options = configuration.Get<ApplicationOptions>()
                ?? throw new InvalidOperationException("Unabled loading appsettings.json!");

using var connect = new SqlConnection (options.ConnectionString);
connect.Open ();

// Вариант 1
var sql = "select top 10 * from journal";
var command = new SqlCommand (sql, connect);

/*
var reader = command.ExecuteReader();
var position = 1;

while(reader.Read ())
{
    Console.WriteLine ($"{position} - {reader[0]}, {reader[1]}");
    position++;
}
*/

// Вариант 2
var adapter = new SqlDataAdapter(command);
var dataset = new DataSet();
adapter.Fill(dataset);

var table = dataset.Tables[0];
for(int position = 0; position < table.Rows.Count; position++)
{
    // Создаем доменную модель
    var dto = new JournalRowDto();

    // Наполняем данными
    dto.Period = Convert.ToDateTime( table.Rows[position][ "dater" ]); // dater
    dto.Quantity = Convert.ToDouble( table.Rows[position]["quantity"] );
    dto.Price = Convert.ToDouble( table.Rows[position]["price"] );

    Console.WriteLine(dto);
}



// Пауза    
while (true)
{
    await Task.Delay(TimeSpan.FromHours(1));
}

