//using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SubastaMaestra.Models.Utils

{
    public class ApiHelper 
    {
        private static readonly HttpClient _httpClient = new();


        public static async Task<string> PostAsync(string url, object objectToPass) 
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(objectToPass);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Operation failed
                    string errorMessage = "Operation failed. Status code: " + response.StatusCode;
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it or return an error message.
                string errorMessage = "An error occurred: " + ex.Message;
                return errorMessage;
            }
        }
        public static async Task<List<T>> GetListAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // If the request was successful, read the content as a string
                    var content = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON content into a list of objects of type T
                    List<T> result = JsonConvert.DeserializeObject<List<T>>(content);
                    return result;
                }
                else
                {
                    // Operation failed
                    string errorMessage = "Operation failed. Status code: " + response.StatusCode;
                    return null; // You can handle this error case as needed
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it or return an error message.
                string errorMessage = "An error occurred: " + ex.Message;
                return null; // You can handle this error case as needed
            }
        }
        public static async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // If the request was successful, read the content as a string
                    var content = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON content into object of type T
                    if (typeof(T) == typeof(string))
                    {
                        // If T is string, return the content as is
                        return (T)(object)content;
                    }
                    else
                    {
                        // Deserialize JSON content into object of type T
                        T result = JsonConvert.DeserializeObject<T>(content);
                        return result;
                    }
                }
                else
                {
                    // Operation failed
                    string errorMessage = "Operation failed. Status code: " + response.StatusCode;
                    return default(T); // You can handle this error case as needed
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it or return an error message.
                string errorMessage = "An error occurred: " + ex.Message;
                return default(T); // You can handle this error case as needed
            }
        }
        public static async Task<string> UpdateAsync(string url, object objectToUpdate)
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(objectToUpdate);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, content); // Assuming you want to use the HTTP PUT method for updates.

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Operation failed
                    string errorMessage = "Update operation failed. Status code: " + response.StatusCode;
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it or return an error message.
                string errorMessage = "An error occurred during the update: " + ex.Message;
                return errorMessage;
            }
        }

        public static async Task<string> DeleteAsync(string url)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // Operation failed
                    string errorMessage = "Operation failed. Status code: " + response.StatusCode;
                    return errorMessage;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately, e.g., log it or return an error message.
                string errorMessage = "An error occurred: " + ex.Message;
                return errorMessage;
            }
        }

    }
}

