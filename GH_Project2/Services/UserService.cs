using GH_Project2.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using GH_Project2.Models.DTOs;

namespace GH_Project2.Services
{
    public class UserService:IUserService
    {
        private readonly HttpClient _httpClient;
        private string MyToken= "fe2506b7c0cb8eb520e957a770b697955164706a247f0e75baead43936d83420";
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var json = JsonSerializer.Serialize(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://gorest.co.in/public/v2/users", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var createdUser = JsonSerializer.Deserialize<User>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return createdUser;
            }
            else
            {
                Console.WriteLine($"Error creating user: {response.StatusCode}");
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization  = new AuthenticationHeaderValue("Bearer", MyToken);

            var response = await _httpClient.DeleteAsync($"https://gorest.co.in/public/v2/users/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var response = await _httpClient.GetAsync("https://gorest.co.in/public/v2/users");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<List<User>>(content, options);

                return result;
            }
            else
            {
                Console.WriteLine("Error fetching data from GoRest API: " + response.StatusCode);
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://gorest.co.in/public/v2/users/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<User>(content, options);
                return result;
            }
            else
            {
                Console.WriteLine($"Error fetching user data from GoRest API for id: {id}, Status Code: {response.StatusCode}");
                return null;
            }
        }

        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users = null;

            HttpResponseMessage response = await _httpClient.GetAsync("https://gorest.co.in/public/v2/users");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<UserApiResponse>(content, options);
                users = result.Data;
            }
            else
            {
                Console.WriteLine("Error fetching data from GoRest API: " + response.StatusCode);
            }

            return users;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            UserDTO userdto =new ()
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                gender = user.Gender,
                status = user.Status
            };
            var json = JsonSerializer.Serialize(userdto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", MyToken);

            var response = await _httpClient.PutAsync($"https://gorest.co.in/public/v2/users/{user.Id}", content);
            var KKK = response.StatusCode;

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var updatedUser = JsonSerializer.Deserialize<User>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return updatedUser;
            }
            else
            {
                Console.WriteLine($"Error updating user: {response.StatusCode}");
                return null;
            }
        }
    }
}
