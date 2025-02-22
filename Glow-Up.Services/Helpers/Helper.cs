using Glow_Up.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glow_Up.Services.Helpers
{
    public static class Helper
    {
        public static MediaType GetMediaType(string contentType)
        {
            return contentType switch
            {
                "image/jpeg" => MediaType.Image,
                "image/png" => MediaType.Image,
                "image/gif" => MediaType.Image,
                "video/mp4" => MediaType.Video,
                "video/mpeg" => MediaType.Video,
                "application/pdf" => MediaType.Document,
                "application/msword" => MediaType.Document,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => MediaType.Document,
                _ => MediaType.Other
            };
        }

        public static string FormatDate(DateTime postDate)
        {
            var timeSpan = DateTime.UtcNow - postDate;

            if (timeSpan.TotalSeconds < 60)
                return "Just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} min";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} h";
            if (timeSpan.TotalDays == 1)
                return "Yesterday";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} d";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} w";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} m";

            return $"{(int)(timeSpan.TotalDays / 365)} y";
        }
    }
}
