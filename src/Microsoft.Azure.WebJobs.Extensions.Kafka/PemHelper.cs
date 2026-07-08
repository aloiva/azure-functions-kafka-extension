// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Azure.WebJobs.Extensions.Kafka
{
    /// <summary>
    /// Utility methods for sanitizing and extracting PEM-encoded certificates and keys.
    /// </summary>
    internal static class PemHelper
    {
        /// <summary>
        /// Sanitizes a PEM certificate/key string by converting literal escape sequences
        /// (e.g. backslash-n from app settings) into actual newlines and normalizing the format.
        /// </summary>
        public static string SanitizePem(string pemValue)
        {
            if (string.IsNullOrWhiteSpace(pemValue))
            {
                return pemValue;
            }

            // Replace literal escape sequences (two-character strings) with actual characters.
            // Order matters: replace \r\n first to avoid double-replacement.
            string cleaned = pemValue
                .Replace("\\r\\n", "\n")
                .Replace("\\n", "\n")
                .Replace("\\r", "\n");

            // Normalize real line endings
            cleaned = cleaned
                .Replace("\r\n", "\n")
                .Replace("\r", "\n");

            // Split, trim each line, remove empty lines, reassemble
            var lines = cleaned.Split('\n')
                .Select(l => l.Trim())
                .Where(l => !string.IsNullOrEmpty(l))
                .ToList();

            return string.Join("\n", lines);
        }

        /// <summary>
        /// Extracts a PEM section (e.g. CERTIFICATE, PRIVATE KEY) from a PEM string,
        /// sanitizing literal escape sequences before extraction.
        /// </summary>
        public static string ExtractSection(string pemString, string sectionName)
        {
            if (string.IsNullOrEmpty(pemString))
            {
                return null;
            }

            // Sanitize first so the regex can match properly
            string sanitized = SanitizePem(pemString);

            var regex = new Regex(
                $"-----BEGIN {sectionName}-----(.*?)-----END {sectionName}-----",
                RegexOptions.Singleline);

            var match = regex.Match(sanitized);
            if (match.Success)
            {
                return match.Value;
            }

            return null;
        }

        /// <summary>
        /// Extracts a CERTIFICATE PEM block from the given string.
        /// </summary>
        public static string ExtractCertificate(string pemString)
        {
            return ExtractSection(pemString, "CERTIFICATE");
        }

        /// <summary>
        /// Extracts a PRIVATE KEY PEM block from the given string.
        /// </summary>
        public static string ExtractPrivateKey(string pemString)
        {
            return ExtractSection(pemString, "PRIVATE KEY");
        }
    }
}
