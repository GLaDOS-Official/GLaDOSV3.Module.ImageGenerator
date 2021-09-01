using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using GLaDOSV3.Services;
using ImageMagick;
using Newtonsoft.Json.Linq;

namespace GLaDOSV3.Module.ImageGenerator
{
    public class GeneratorService
    {
        public bool Fail;
        private const string HtmlSplit = " ";
        public GeneratorService()
        {
            if (!OperatingSystem.IsWindows())  {this.Fail = false; return;}
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), $"Binaries{Path.DirectorySeparatorChar}wkhtmltoimage.exe"))) { LoggingService.Log(LogSeverity.Error, "ImageGenerator", "wkhtmltoimage.exe not found!"); this.Fail = true; };
        }
        public string FixUrl(string url)
        {
            url = url.Contains('?') ? url.Split('?')[0] : url;
            return url.Replace("/", "%2F").Replace(":", "%3A");
        }

        public Task<MemoryStream> Shit(string[] items, ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var item = items.Aggregate(string.Empty, (current, type) => current + type + ", ");
                var html = File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(),
                                                              $"images{Path.DirectorySeparatorChar}shit.html")).GetAwaiter().GetResult().Replace("REPLACEWITHITEM", item.Remove(item.Length - 2)).Replace("REPLACECORRECTPLURAL", items.Length > 1 ? "are" : "is");
                var jpgBytes = Exec(html).GetAwaiter().GetResult();
                return Task.FromResult(new MemoryStream(jpgBytes));
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<MemoryStream> Delete(string item, ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var html = File.ReadAllTextAsync(Path.Combine(Directory.GetCurrentDirectory(),
                                                              $"images{Path.DirectorySeparatorChar}delete.html")).GetAwaiter().GetResult().Replace("REPLACEWITHITEM", item);
                var jpgBytes = Exec(html).GetAwaiter().GetResult();
                return Task.FromResult(new MemoryStream(jpgBytes));
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<MemoryStream> Beautiful(ICommandContext context, string url)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                using var images = new MagickImageCollection();
                using HttpClient hc = new HttpClient();
                MagickImage image1 = new MagickImage($".{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}beautiful.png");
                MagickImage image2 = new MagickImage(hc.GetByteArrayAsync(url.Replace(".gif", ".png")).GetAwaiter().GetResult());
                MagickImage image3 = new MagickImage(image2);
                image1.Alpha(AlphaOption.Set);

                image2.InterpolativeResize(90, 112 + 7, PixelInterpolateMethod.Bilinear);
                image2.Page = new MagickGeometry("+256+20");
                image3.InterpolativeResize(90, 105 + 7, PixelInterpolateMethod.Bilinear);
                image3.Page = new MagickGeometry("+257+220");
                images.Add(image3);
                images.Add(image2);
                images.Add(image1);
                var result = images.Merge();
                using var stream = new MemoryStream();
                result.Write(stream);
                byte[] bytes;
                bytes = stream.ToArray();
                return Task.FromResult(new MemoryStream(bytes));
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<string> Pat(ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var result = NekosDevApi("sfw/gif/pat").GetAwaiter().GetResult();
                return result == null ? null : Task.FromResult(result);
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<string> Kiss(ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var result = NekosDevApi("sfw/gif/kiss").GetAwaiter().GetResult();
                return result == null ? null : Task.FromResult(result);
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<string> Tickle(ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var result = NekosDevApi("sfw/gif/tickle").GetAwaiter().GetResult();
                return result == null ? null : Task.FromResult(result);
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<string> Poke(ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var result = NekosDevApi("sfw/gif/poke").GetAwaiter().GetResult();
                return result == null ? null : Task.FromResult(result);
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<string> Slap(ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var result = NekosDevApi("sfw/gif/slap").GetAwaiter().GetResult();
                return result == null ? null : Task.FromResult(result);
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<string> Cuddle(ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var result = NekosDevApi("sfw/gif/cuddle").GetAwaiter().GetResult();
                return result == null ? null : Task.FromResult(result);
            }
            finally
            {
                typing.Dispose();
            }
        }
        public Task<string> Hug(ICommandContext context)
        {
            IDisposable typing = context.Channel.EnterTypingState();
            try
            {
                var result = NekosDevApi("sfw/gif/hug").GetAwaiter().GetResult();
                return result == null ? null : Task.FromResult(result);
            }
            finally
            {
                typing.Dispose();
            }
        }
        //https://cdn.blackofworld.fun/neko/sfw/gif/kiss/kiss_041.gif
        public static async Task<string> NekosDevApi(string path)
        {
            using HttpClient hc = new HttpClient();
            JObject json = JObject.Parse(hc.GetStringAsync($"https://api.nekos.dev/api/v3/images/{path}").GetAwaiter().GetResult());
            if (!(bool)json["data"]["status"]["success"].ToObject(typeof(bool))) return null;
            var url = json["data"]["response"]["url"].ToString();
            url = "https://cdn.blackofworld.fun/neko/" + url[(url.IndexOf("/v3/") + 4)..];
            return url;
        }
        public static async Task<byte[]> Exec(string html, int width = 0, int height = 0) // Custom wrapper!!!
        {
            var e = Process.Start(new ProcessStartInfo
            {
                Arguments = $"-q --width {width} --height {height} -f jpeg  - -",
                FileName = Path.Combine(Directory.GetCurrentDirectory(),
                    $"Binaries{Path.DirectorySeparatorChar}wkhtmltoimage.exe"),
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            });
            await using (StreamWriter stream = e.StandardInput)
            {
                var htmlcontent = Encoding.UTF8.GetBytes(html);
                await stream.BaseStream.WriteAsync(htmlcontent, 0, htmlcontent.Length).ConfigureAwait(false);
                await stream.WriteLineAsync().ConfigureAwait(false);
                await stream.BaseStream.FlushAsync().ConfigureAwait(false);
                stream.Close();
            }
            await using (MemoryStream stream = new MemoryStream())
            {
                await e.StandardOutput.BaseStream.CopyToAsync(stream);
                await e.StandardOutput.BaseStream.FlushAsync().ConfigureAwait(false);
                e.StandardOutput.Close();
                return stream.ToArray();
            }
        }

    }
}