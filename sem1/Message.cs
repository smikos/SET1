﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace sem1
{
    internal class Message
    {
        public string FromName { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        // Метод для сериализации в JSON
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        // Статический метод для десериализации JSON в объект MyMessage
        public static Message FromJson(string json)
        {
            return JsonSerializer.Deserialize<Message>(json);
        }

    }
}
