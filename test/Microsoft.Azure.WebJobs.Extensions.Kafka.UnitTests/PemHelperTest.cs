// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Xunit;

namespace Microsoft.Azure.WebJobs.Extensions.Kafka.UnitTests
{
    public class PemHelperTest
    {
        private const string ValidCert =
            "-----BEGIN CERTIFICATE-----\n" +
            "MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ\n" +
            "RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\n" +
            "VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX\n" +
            "DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y\n" +
            "-----END CERTIFICATE-----";

        private const string ValidCertSingleLine =
            "-----BEGIN CERTIFICATE-----" +
            "MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ" +
            "RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD" +
            "VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX" +
            "DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y" +
            "-----END CERTIFICATE-----";

        // Simulates what app settings look like: literal \n characters in a single string
        private const string CertWithLiteralBackslashN =
            "-----BEGIN CERTIFICATE-----\\n" +
            "MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ\\n" +
            "RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\\n" +
            "VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX\\n" +
            "DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y\\n" +
            "-----END CERTIFICATE-----";

        private const string CertWithLiteralBackslashRN =
            "-----BEGIN CERTIFICATE-----\\r\\n" +
            "MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ\\r\\n" +
            "RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\\r\\n" +
            "VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX\\r\\n" +
            "DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y\\r\\n" +
            "-----END CERTIFICATE-----";

        private const string CertWithLiteralBackslashR =
            "-----BEGIN CERTIFICATE-----\\r" +
            "MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ\\r" +
            "RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\\r" +
            "VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX\\r" +
            "DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y\\r" +
            "-----END CERTIFICATE-----";

        private const string ExpectedSanitizedCert =
            "-----BEGIN CERTIFICATE-----\n" +
            "MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ\n" +
            "RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\n" +
            "VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX\n" +
            "DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y\n" +
            "-----END CERTIFICATE-----";

        #region SanitizePem Tests

        [Fact]
        public void SanitizePem_NullInput_ReturnsNull()
        {
            Assert.Null(PemHelper.SanitizePem(null));
        }

        [Fact]
        public void SanitizePem_EmptyInput_ReturnsEmpty()
        {
            Assert.Equal("", PemHelper.SanitizePem(""));
        }

        [Fact]
        public void SanitizePem_WhitespaceOnly_ReturnsWhitespace()
        {
            Assert.Equal("   ", PemHelper.SanitizePem("   "));
        }

        [Fact]
        public void SanitizePem_ValidCert_PreservesStructure()
        {
            var result = PemHelper.SanitizePem(ValidCert);
            Assert.Equal(ExpectedSanitizedCert, result);
        }

        [Fact]
        public void SanitizePem_LiteralBackslashN_ConvertedToNewlines()
        {
            var result = PemHelper.SanitizePem(CertWithLiteralBackslashN);
            Assert.Equal(ExpectedSanitizedCert, result);
            Assert.DoesNotContain("\\n", result);
        }

        [Fact]
        public void SanitizePem_LiteralBackslashRN_ConvertedToNewlines()
        {
            var result = PemHelper.SanitizePem(CertWithLiteralBackslashRN);
            Assert.Equal(ExpectedSanitizedCert, result);
            Assert.DoesNotContain("\\r", result);
            Assert.DoesNotContain("\\n", result);
        }

        [Fact]
        public void SanitizePem_LiteralBackslashR_ConvertedToNewlines()
        {
            var result = PemHelper.SanitizePem(CertWithLiteralBackslashR);
            Assert.Equal(ExpectedSanitizedCert, result);
            Assert.DoesNotContain("\\r", result);
        }

        [Fact]
        public void SanitizePem_RealCRLF_NormalizedToLF()
        {
            var input = "-----BEGIN CERTIFICATE-----\r\nBODY\r\n-----END CERTIFICATE-----";
            var result = PemHelper.SanitizePem(input);
            Assert.Equal("-----BEGIN CERTIFICATE-----\nBODY\n-----END CERTIFICATE-----", result);
            Assert.DoesNotContain("\r", result);
        }

        [Fact]
        public void SanitizePem_TrimsWhitespaceFromLines()
        {
            var input = "  -----BEGIN CERTIFICATE-----  \n  BODY  \n  -----END CERTIFICATE-----  ";
            var result = PemHelper.SanitizePem(input);
            Assert.Equal("-----BEGIN CERTIFICATE-----\nBODY\n-----END CERTIFICATE-----", result);
        }

        [Fact]
        public void SanitizePem_RemovesEmptyLines()
        {
            var input = "-----BEGIN CERTIFICATE-----\n\nBODY\n\n-----END CERTIFICATE-----";
            var result = PemHelper.SanitizePem(input);
            Assert.Equal("-----BEGIN CERTIFICATE-----\nBODY\n-----END CERTIFICATE-----", result);
        }

        #endregion

        #region ExtractCertificate Tests

        [Fact]
        public void ExtractCertificate_NullInput_ReturnsNull()
        {
            Assert.Null(PemHelper.ExtractCertificate(null));
        }

        [Fact]
        public void ExtractCertificate_EmptyInput_ReturnsNull()
        {
            Assert.Null(PemHelper.ExtractCertificate(""));
        }

        [Fact]
        public void ExtractCertificate_ValidCert_ReturnsCertBlock()
        {
            var result = PemHelper.ExtractCertificate(ValidCert);
            Assert.NotNull(result);
            Assert.StartsWith("-----BEGIN CERTIFICATE-----", result);
            Assert.EndsWith("-----END CERTIFICATE-----", result);
        }

        [Fact]
        public void ExtractCertificate_LiteralBackslashN_ReturnsSanitizedCert()
        {
            var result = PemHelper.ExtractCertificate(CertWithLiteralBackslashN);
            Assert.NotNull(result);
            Assert.DoesNotContain("\\n", result);
            Assert.StartsWith("-----BEGIN CERTIFICATE-----", result);
            Assert.EndsWith("-----END CERTIFICATE-----", result);
        }

        [Fact]
        public void ExtractCertificate_NoCertBlock_ReturnsNull()
        {
            var result = PemHelper.ExtractCertificate("just some random text");
            Assert.Null(result);
        }

        [Fact]
        public void ExtractCertificate_CertWithExtraText_ExtractsOnlyCertBlock()
        {
            var input = "EXTRA TEXT BEFORE\n" + ValidCert + "\nEXTRA TEXT AFTER";
            var result = PemHelper.ExtractCertificate(input);
            Assert.NotNull(result);
            Assert.StartsWith("-----BEGIN CERTIFICATE-----", result);
            Assert.EndsWith("-----END CERTIFICATE-----", result);
            Assert.DoesNotContain("EXTRA TEXT", result);
        }

        #endregion

        #region ExtractPrivateKey Tests

        [Fact]
        public void ExtractPrivateKey_NullInput_ReturnsNull()
        {
            Assert.Null(PemHelper.ExtractPrivateKey(null));
        }

        [Fact]
        public void ExtractPrivateKey_ValidKey_ReturnsKeyBlock()
        {
            var keyPem = "-----BEGIN PRIVATE KEY-----\nKEYDATA\n-----END PRIVATE KEY-----";
            var result = PemHelper.ExtractPrivateKey(keyPem);
            Assert.NotNull(result);
            Assert.StartsWith("-----BEGIN PRIVATE KEY-----", result);
            Assert.EndsWith("-----END PRIVATE KEY-----", result);
        }

        [Fact]
        public void ExtractPrivateKey_LiteralBackslashN_ReturnsSanitizedKey()
        {
            var keyPem = "-----BEGIN PRIVATE KEY-----\\nKEYDATA\\n-----END PRIVATE KEY-----";
            var result = PemHelper.ExtractPrivateKey(keyPem);
            Assert.NotNull(result);
            Assert.DoesNotContain("\\n", result);
            Assert.StartsWith("-----BEGIN PRIVATE KEY-----", result);
        }

        [Fact]
        public void ExtractPrivateKey_NoKeyBlock_ReturnsNull()
        {
            var result = PemHelper.ExtractPrivateKey("no key here");
            Assert.Null(result);
        }

        #endregion

        #region Combined Cert + Key Extraction

        [Fact]
        public void ExtractCertificate_FromCombinedPem_ReturnsOnlyCert()
        {
            var combined = ValidCert + "\n-----BEGIN PRIVATE KEY-----\nKEYDATA\n-----END PRIVATE KEY-----";
            var result = PemHelper.ExtractCertificate(combined);
            Assert.NotNull(result);
            Assert.StartsWith("-----BEGIN CERTIFICATE-----", result);
            Assert.DoesNotContain("PRIVATE KEY", result);
        }

        [Fact]
        public void ExtractPrivateKey_FromCombinedPem_ReturnsOnlyKey()
        {
            var combined = ValidCert + "\n-----BEGIN PRIVATE KEY-----\nKEYDATA\n-----END PRIVATE KEY-----";
            var result = PemHelper.ExtractPrivateKey(combined);
            Assert.NotNull(result);
            Assert.StartsWith("-----BEGIN PRIVATE KEY-----", result);
            Assert.DoesNotContain("CERTIFICATE", result);
        }

        #endregion
    }
}
