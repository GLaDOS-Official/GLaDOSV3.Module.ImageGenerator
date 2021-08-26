﻿using System;
using System.Reflection;
using System.Runtime.Loader;
using Discord.Commands;
using Discord.WebSocket;
using GLaDOSV3.Helpers;
using GLaDOSV3.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GLaDOSV3.Module.ImageGenerator
{
    public class ModuleInfo : GladosModule
    {
        public override string Name => "ImageGenerator";

        public override string Version => "0.0.0.1";

        public override string Author  => "BlackOfWorld#8125";

        public override Type[] Services(DiscordShardedClient discord, CommandService commands, BotSettingsHelper<string> config, IServiceCollection provider) => new[] { typeof(GeneratorService) };
       
    }
}
