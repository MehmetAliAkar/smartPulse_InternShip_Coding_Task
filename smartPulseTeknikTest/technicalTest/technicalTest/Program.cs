using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // API'den veriyi almak için GET isteği gönderelim
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync("https://seffaflik.epias.com.tr/transparency/service/market/intra-day-trade-history?endDate=2023-06-15&startDate=2023-06-15");

            if (response.IsSuccessStatusCode)
            {
                
                string responseBody = await response.Content.ReadAsStringAsync();//apiden gelen veriyi JSON olarak okumamızı sağlar

                // JSON veriyi sınıflara dönüştürelim ve liste oluşturalım
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var jsonData = JsonSerializer.Deserialize<JsonData>(responseBody, options);

                // jsonData.Data null değer kontrolü yapılmalıdır
                if (jsonData.Data != null)//datanın null değer olup olmadığı kontrol eder
                {
                    List<MyData> dataList = jsonData.Data;

                    
                    foreach (var data in dataList)//elde edilen listeyi yazdırma
                    {
                        Console.WriteLine($"ID: {data.Id}, Date: {data.Date}, Contract: {data.Contract}, Price: {data.Price}, Quantity: {data.Quantity}");
                    }
                }
                else//apiden veri alınamaz ve ya gelen değer null ise
                {
                    Console.WriteLine("API'den veri alınamadı veya veri boş.");
                }
            }
            else//eğer apiden GET isteği başarılı bir şekilde sağlanamamışsa hata kodu döndür
            {
                Console.WriteLine("API isteği başarısız oldu. Hata kodu: " + response.StatusCode);
            }
        }
        Console.Read();
    }
}

class JsonData
{
    public List<MyData> Data { get; set; }
}

class MyData
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Contract { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
