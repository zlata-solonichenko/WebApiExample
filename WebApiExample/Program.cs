﻿using System.Net.Http.Json;
using System.Text.Json;
using WebApiExample;

var posts = default(List<Post>);

//запрос GET
using (var client = new HttpClient())
{
    var result = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
    result.EnsureSuccessStatusCode(); //проверка что код 2xx

    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    posts = JsonSerializer.Deserialize<List<Post>>(await result.Content.ReadAsStringAsync(), options);
}

// foreach (var post in posts.GroupBy(p=>p.UserId).Select(g=>g.Take(1)).Take(10))
// {
//     Console.WriteLine(JsonSerializer.Serialize(post));
// }

// запрос Get с comment
using (var client = new HttpClient())
{
    var result = await client.GetFromJsonAsync<List<Comment>>("https://jsonplaceholder.typicode.com/posts/1/comments");

    foreach (var comment in result.GroupBy(p => p.PostId).Select(g => g.TakeLast(2)))
    {
        Console.WriteLine(JsonSerializer.Serialize(comment));
    }
}

//запрос POST
var post = default(Post);
using (var client = new HttpClient())
{
    //отправляемый объект
    var newPost = new Post { Id = 1, UserId = 101, Title = "et setera", Body = "la la la la la la" };

    //отправить запрос
    var response = await client.PostAsJsonAsync("https://jsonplaceholder.typicode.com/posts", newPost);
    response.EnsureSuccessStatusCode();

    post = await response.Content.ReadFromJsonAsync<Post>();
}

Console.WriteLine($"Id: {post?.Id} - UserId: {post?.UserId} - Title: {post?.Title} - Body: {post.Body}");

//запрос PUT
using (var client = new HttpClient())
{
    //обновить полностью информацию
    var response = await client.PutAsJsonAsync("https://jsonplaceholder.typicode.com/posts/1",
        new Post() { Id = 1, UserId = 1, Title = "update album", Body = "et setera" });

    response.EnsureSuccessStatusCode();

    post = await response.Content.ReadFromJsonAsync<Post>();
    //Console.WriteLine($" {post.UserId} - {post.Body}");
}

//запрос DELETE

using (var client = new HttpClient())
{
    var response = await client.DeleteAsync("https://jsonplaceholder.typicode.com/posts/1");
    Console.WriteLine(response.EnsureSuccessStatusCode());

    var jsonResponse = await response.Content.ReadAsStringAsync();
    
    //posts = JsonSerializer.Deserialize<List<Post>>(jsonResponse);

    //Console.WriteLine(posts);
    //Console.WriteLine($"{jsonResponse}\n");
}
