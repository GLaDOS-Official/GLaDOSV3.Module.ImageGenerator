using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Discord;
using Discord.Commands;
using GLaDOSV3.Attributes;

namespace GLaDOSV3.Module.ImageGenerator
{
    public class ImageGeneratorModule : ModuleBase<ShardedCommandContext>
    {
        private readonly GeneratorService _service;
        private readonly string[] imageFormats = { "jpg", "jpeg", "bmp", "png" };

        private readonly string[] patStrings = { "Pat pat!", "{mention} I heard that pats are great and healthy", "Pattu?" };
        private readonly string[] hugStrings = { "Hey {mention}! You look like you could use a hug ❤", "Here {mention}, have a hug.", "Don't worry, I'm here", "Don't worry, you're not lonely" };
        private readonly string[] cuddleStrings = { };
        private readonly string[] kissStrings = { };
        private readonly string[] slapStrings = { "{mention} the fuck did you say to me you little bitch?!", "Get slapped!", "No apologies..." };
        private readonly string[] tickleStrings = { };
        private readonly string[] pokeStrings = { };
        public ImageGeneratorModule(GeneratorService service) => this._service = service;

        private bool HasImageExtension(string path) => this.imageFormats.Any(path.EndsWith);

        [Command("delete", RunMode = RunMode.Async)]
        [Remarks("delete")]
        [Summary("delete")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Delete([Remainder] string text)
        {
            if (!this._service.Fail)
                await Context.Channel.SendFileAsync(this._service.Delete(text, Context).GetAwaiter().GetResult(), "delet.jpg");
            else
                await Context.Channel.SendMessageAsync("There was an error... Check the logs!");
        }
        [Command("shit", RunMode = RunMode.Async)]
        [Remarks("shit <who>")]
        [Summary("shit")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Shit([Remainder] string text)
        {
            if (!this._service.Fail)
                await Context.Channel.SendFileAsync(this._service.Shit(text.Split(','), Context).GetAwaiter().GetResult(), "shit.jpg");
            else
                await Context.Channel.SendMessageAsync("There was an error... Check the logs!");
        }
        [Command("mc", RunMode = RunMode.Async)]
        [Remarks("mc <name>")]
        [Summary("Achievement get!")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Minecraft([Remainder] string text) => await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/mc/Achievement+Get!/{text}");
        [Command("threats", RunMode = RunMode.Async)]
        [Remarks("threats [mention/userid/file]")]
        [Summary("The 3 biggest threats to society...")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Threats(IUser user = null)
        {
            string url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            if (user != null) url = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png"));
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    url = attach.Url;
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?url={(url.Replace(".gif", ".png"))}&type=threats");
        }
        [Command("beautiful", RunMode = RunMode.Async)]
        [Remarks("beautiful [mention/userid/file]")]
        [Summary("Look at it! It's--- BEAUTIFUL!")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Beautiful(IUser user = null)
        {
            if (user != null)
                await Context.Channel.SendFileAsync(this._service.Beautiful(Context, user.GetAvatarUrl(size: 1024) ?? user.GetDefaultAvatarUrl()).GetAwaiter().GetResult(), "beautiful.jpg");
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    await Context.Channel.SendFileAsync(this._service.Beautiful(Context, attach.Url).GetAwaiter().GetResult(), "beautiful.jpg");
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            else
                await Context.Channel.SendFileAsync(this._service.Beautiful(Context, Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).GetAwaiter().GetResult(), "beautiful.jpg");
        }
        [Command("baguette", RunMode = RunMode.Async)]
        [Remarks("baguette [mention/userid/file]")]
        [Summary("Yummy!")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Baguette(IUser user = null)
        {
            string url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            if (user != null) url = (user.GetAvatarUrl(size: 1024) ?? user.GetDefaultAvatarUrl());
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    url = attach.Url;
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?url={(url.Replace(".gif", ".png"))}&type=baguette");
        }
        [Command("clyde", RunMode = RunMode.Async)]
        [Remarks("clyde <clyde>")]
        [Summary("Clyde? What the are you saying again?")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Clyde([Remainder] string clyde) => await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?text={clyde}&type=clyde");
        [Command("relationship", RunMode = RunMode.Async)]
        [Remarks("relationship <userid/mention> [userid/mention]")]
        [Summary("OwO Who's that?")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Relationship(IUser user, IUser user2 = null)
        {
            if (user2 == null) user2 = Context.Client.CurrentUser;
            var userUrl = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png"));
            var userUrl2 = ((user2.GetAvatarUrl() ?? user2.GetDefaultAvatarUrl()).Replace(".gif", ".png"));

            userUrl = this._service.FixUrl(userUrl);
            userUrl2 = this._service.FixUrl(userUrl2);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?user1={userUrl}&user2={userUrl2}&type=ship");
        }
        [Command("captcha", RunMode = RunMode.Async)]
        [Remarks("captcha [mention/userid/file]")]
        [Summary("Please verify to continue...")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Captcha(IUser user = null)
        {
            var url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            var username = HttpUtility.UrlEncode(Context.User.Username);
            if (user != null)
            {
                url = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png")); ;
                username = HttpUtility.UrlEncode(user.Username);
            }
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();

                if (this.HasImageExtension(attach.Url))
                {
                    url = attach.Url;
                    username = Path.GetFileNameWithoutExtension(attach.Filename);
                }
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?url={url}&username={username}&type=captcha");
        }
        [Command("whowouldwin", RunMode = RunMode.Async)]
        [Remarks("whowouldwin <userid/mention> [userid/mention]")]
        [Summary("Who would win?")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task WhoWouldWin(IUser user, IUser user2 = null)
        {
            if (user2 == null) user2 = Context.Client.CurrentUser;
            var userUrl = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png"));
            var userUrl2 = ((user2.GetAvatarUrl() ?? user2.GetDefaultAvatarUrl()).Replace(".gif", ".png"));

            userUrl = this._service.FixUrl(userUrl);
            userUrl2 = this._service.FixUrl(userUrl2);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?user1={(userUrl)}&user2={userUrl2}&type=whowouldwin");
        }
        [Command("changemymind", RunMode = RunMode.Async)]
        [Remarks("changemymind <text>")]
        [Summary("Change my mind bruh!")]
        [Timeout(5, 30, Measure.Seconds)]
        [Alias("cmm")]
        public async Task ChangeMyMind([Remainder] string cmm) => await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?text={(cmm.Length >= 100 ? cmm.Substring(0, 100) : cmm)}&type=changemymind");
        [Command("jpeg", RunMode = RunMode.Async)]
        [Remarks("jpeg  [mention/userid/file]")]
        [Summary("Jpegify")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Jpegify(IUser user = null)
        {
            string url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            if (user != null) url = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png"));
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    url = attach.Url;
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?url={url}&type=jpeg");
        }
        [Command("lolice", RunMode = RunMode.Async)]
        [Remarks("lolice  [mention/userid/file]")]
        [Summary("Anime loli police chief")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Lolice(IUser user = null)
        {
            string url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            if (user != null) url = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png"));
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    url = attach.Url;
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?url={url}&type=lolice");
        }
        [Command("kannafy", RunMode = RunMode.Async)]
        [Remarks("kannafy <text>")]
        [Summary("Kanna OwO")]
        [Timeout(5, 30, Measure.Seconds)]
        [Alias("kannagen")]
        public async Task Kannagen([Remainder] string text)
        {
            const int splitPerChar = 10;
            const int splitPerUpperChar = 8;
            if (text.Length >= 45) text = text.Substring(0, 45);

            string[] split = text.Split(' ');
            for (int i = 0; i < split.Length; i++)
            {
                int splitNum = char.IsUpper(split[i][^1]) ? splitPerUpperChar : splitPerChar;
                if (split[i].Length < splitNum) continue;
                for (int j = splitNum; j < split[i].Length; j += splitNum) split[i] = split[i].Insert(j, " ");
            }
            string result = string.Join(' ', split);

            Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?text={result}&type=kannagen").GetAwaiter().GetResult();
        }
        [Command("pat", RunMode = RunMode.Async)]
        [Remarks("pat [user]")]
        [Summary("Pat someone for being cute :3")]
        [Alias("headpat", "pattu")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Pat(IUser user = null) => await Context.Channel.SendMessageAsync(await this._service.Pat(Context));
        [Command("kiss", RunMode = RunMode.Async)]
        [Remarks("kiss [user]")]
        [Summary("Kiss someone you love 😘")]
        [Alias("kissu")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Kiss(IUser user = null) => await Context.Channel.SendMessageAsync(await this._service.Kiss(Context));
        [Command("tickle", RunMode = RunMode.Async)]
        [Remarks("tickle [user]")]
        [Summary("Tickle his funny bones 😏")]
        [Alias("tickly")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Tickle(IUser user = null) => await Context.Channel.SendMessageAsync(await this._service.Tickle(Context));
        [Command("poke", RunMode = RunMode.Async)]
        [Remarks("poke [user]")]
        [Summary("Hey you! I need your attention!")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Poke(IUser user = null) => await Context.Channel.SendMessageAsync(await this._service.Poke(Context));
        [Command("slap", RunMode = RunMode.Async)]
        [Remarks("slap [user]")]
        [Summary("Slap them!")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Slap(IUser user = null) => await Context.Channel.SendMessageAsync(await this._service.Slap(Context));
        [Command("cuddle", RunMode = RunMode.Async)]
        [Remarks("cuddle [user]")]
        [Summary("Cuddle with your waifu 😍")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Cuddle(IUser user = null) => await Context.Channel.SendMessageAsync(await this._service.Cuddle(Context));
        [Command("hug", RunMode = RunMode.Async)]
        [Remarks("hug [user]")]
        [Summary("Hug someone who really needs it ❤🙌")]
        [Alias("huggu")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Hug(IUser user = null) => await Context.Channel.SendMessageAsync(await this._service.Hug(Context));
        [Command("iphonex", RunMode = RunMode.Async)]
        [Remarks("iphonex [mention/userid/file]")]
        [Summary("Hmm what can we fit into iphonex screen this time?")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task PhoneX(IUser user = null)
        {
            string url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            if (user != null) url = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png"));
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    url = attach.Url;
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?url={(url)}&type=iphonex");
        }

        [Command("trap", RunMode = RunMode.Async)]
        [Remarks("trap [mention/userid]")]
        [Summary("Got you! Heheh")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Trap(IUser user) {
            string url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            if (user != null) url = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png"));
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    url = attach.Url;
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?url={url}&author={HttpUtility.UrlEncode(Context.User.Username)}&name={HttpUtility.UrlEncode(user.Username)}&type=trap");
        }
        [Command("trump", RunMode = RunMode.Async)]
        [Remarks("trump <text>")]
        [Summary("New notification on twitter from realDonaldTrump!")]
        [Timeout(5, 30, Measure.Seconds)]
        [Alias("trumptweet")]
        public async Task TrumpTweet([Remainder] string trump)
        {
            if (trump.Length > 33) trump = trump.Insert(34, " ");
            if (trump.Length >= 72) trump = trump.Substring(0, 72);

            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?text={trump}&type=trumptweet");
        }
        [Command("deepfry", RunMode = RunMode.Async)]
        [Remarks("deepfry [mention/userid/file]")]
        [Summary("Did anyone said deepfry?")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Deepfry(IUser user = null)
        {
            string url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            if (user != null) url = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png"));
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    url = attach.Url;
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?url={(url)}&type=deepfry");
        }

        [Command("magik", RunMode = RunMode.Async)]
        [Remarks("magik [mention/userid/file]")]
        [Summary("Did anyone said magik?")]
        [Timeout(5, 30, Measure.Seconds)]
        public async Task Magik(IUser user = null)
        {
            string url = (Context.User.GetAvatarUrl(size: 1024) ?? Context.User.GetDefaultAvatarUrl()).Replace(".gif", ".png");
            if (user != null) url = ((user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()).Replace(".gif", ".png")); 
            else if (Context.Message.Attachments.Count > 0)
            {
                IAttachment attach = Context.Message.Attachments.First();
                if (this.HasImageExtension(attach.Url))
                    url = attach.Url;
                else
                    await this.ReplyAsync("The attachment is not an image!");
            }
            url = this._service.FixUrl(url);
            Random rand = new Random();
            await Context.Channel.SendMessageAsync($"https://cdn.blackofworld.fun/cat/?image={(url.Replace(".gif", ".png"))}&type=magik&intensity={rand.Next(10)}");
        }
    }
}