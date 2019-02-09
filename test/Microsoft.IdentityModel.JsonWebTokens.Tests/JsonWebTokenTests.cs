//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Json.Linq;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.JsonWebTokens.Tests
{
    public class JsonWebTokenTests
    {
        // Test checks to make sure that the JsonWebToken payload is correctly converted to IEnumerable<Claim>.
        [Fact]
        public void GetClaimsFromJObject()
        {
            var context = new CompareContext();
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            var jsonWebTokenString = jsonWebTokenHandler.CreateToken(Default.PayloadString, KeyingMaterial.JsonWebKeyRsa256SigningCredentials);
            var jsonWebToken = new JsonWebToken(jsonWebTokenString);
            var claims = jsonWebToken.Claims;
            IdentityComparer.AreEqual(Default.PayloadClaims, claims, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        // Test checks to make sure that the 'Audiences' claim can be successfully retrieved when multiple audiences are present.
        // It also checks that the rest of the claims match up as well
        [Fact]
        public void GetMultipleAudiences()
        {
            var context = new CompareContext();
            var tokenString = "eyJhbGciOiJSUzI1NiJ9.eyJhdWQiOlsiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UuY29tIiwiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2UxLmNvbSIsImh0dHA6Ly9EZWZhdWx0LkF1ZGllbmNlMi5jb20iLCJodHRwOi8vRGVmYXVsdC5BdWRpZW5jZTMuY29tIiwiaHR0cDovL0RlZmF1bHQuQXVkaWVuY2U0LmNvbSJdLCJleHAiOjE1Mjg4NTAyNzgsImlhdCI6MTUyODg1MDI3OCwiaXNzIjoiaHR0cDovL0RlZmF1bHQuSXNzdWVyLmNvbSIsIm5vbmNlIjoiRGVmYXVsdC5Ob25jZSIsInN1YiI6InVybjpvYXNpczpuYW1zOnRjOlNBTUw6MS4xOm5hbWVpZC1mb3JtYXQ6WDUwOVN1YmplY3ROYW1lIn0.";
            var jsonWebToken = new JsonWebToken(tokenString);
            var jwtSecurityToken = new JwtSecurityToken(tokenString);
            IdentityComparer.AreEqual(jsonWebToken.Claims, jwtSecurityToken.Claims);
            IdentityComparer.AreEqual(jsonWebToken.Audiences, jwtSecurityToken.Audiences, context);
            TestUtilities.AssertFailIfErrors(context);
        }

        [Fact]
        public void TryGetValue()
        {
            var testContext = new CompareContext();
            TestUtilities.WriteHeader($"{this}.TryGetValue");

            Microsoft.IdentityModel.Json.Linq.JObject Header = new Microsoft.IdentityModel.Json.Linq.JObject()
            {
                { "alg", "rsa" },
                { "kid", "123" },
                { "typ", "jwt" },
                { "nullHeader", null }
            };

            Microsoft.IdentityModel.Json.Linq.JObject Payload = new Microsoft.IdentityModel.Json.Linq.JObject
            {
                { "array_value", new Microsoft.IdentityModel.Json.Linq.JArray("1", 1) },
                { "plain_value", "test" },
                { "nested_object",
                    new Microsoft.IdentityModel.Json.Linq.JObject
                    {
                        {"nested_plain_value", "test2" }
                    }
                }
            };


            var jwt = new JsonWebToken(Header.ToString(), Payload.ToString());

            IdentityComparer.AreBoolsEqual(true, jwt.TryGetHeaderValue("alg", out string alg), testContext);
            IdentityComparer.AreBoolsEqual(true, jwt.TryGetHeaderValue("kid", out string kidString), testContext);
            IdentityComparer.AreBoolsEqual(true, jwt.TryGetHeaderValue("kid", out int? kidInt), testContext);
            IdentityComparer.AreBoolsEqual(true, jwt.TryGetHeaderValue("nullHeader", out int? nullVal1), testContext);
            IdentityComparer.AreBoolsEqual(true, jwt.TryGetHeaderValue("nullHeader", out string nullVal2), testContext);
            IdentityComparer.AreBoolsEqual(false, jwt.TryGetPayloadValue("array_value", out int? array1Fails), testContext);
            IdentityComparer.AreBoolsEqual(true, jwt.TryGetPayloadValue("array_value", out string array2), testContext);
            IdentityComparer.AreBoolsEqual(true, jwt.TryGetPayloadValue("nested_object", out string nestedObject), testContext);

            TestUtilities.AssertFailIfErrors(testContext);
        }

        // Time values can be floats, ints, or strings.
        // This test checks to make sure that parsing does not fault in any of the above cases.
        [Theory, MemberData(nameof(ParseTimeValuesTheoryData))]
        public void ParseTimeValues(ParseTimeValuesTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ParseTimeValues", theoryData);
            var jsonWebTokenHandler = new JsonWebTokenHandler();
            try
            {
                var token = new JsonWebToken(theoryData.Header, theoryData.Payload);
                var validFrom = token.ValidFrom;
                var validTo = token.ValidTo;
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ParseTimeValuesTheoryData> ParseTimeValuesTheoryData
        {
            get
            {
                return new TheoryData<ParseTimeValuesTheoryData>
                {
                    // Dates as strings
                    new ParseTimeValuesTheoryData
                    {
                        First = true,
                        Payload = Default.PayloadString,
                        Header = new JObject
                        {
                            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Sha512  },
                            { JwtHeaderParameterNames.Kid, Default.AsymmetricSigningKey.KeyId },
                            { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType }
                        }.ToString(Formatting.None)
                    },
                    // Dates as longs
                    new ParseTimeValuesTheoryData
                    {
                        First = true,
                        Payload = new JObject()
                        {       
                            { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                            { JwtRegisteredClaimNames.GivenName, "Bob" },
                            { JwtRegisteredClaimNames.Iss, Default.Issuer },
                            { JwtRegisteredClaimNames.Aud, Default.Audience },
                            { JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(Default.NotBefore)},
                            { JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(Default.Expires) }
                        }.ToString(Formatting.None),
                        Header = new JObject
                        {
                            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Sha512  },
                            { JwtHeaderParameterNames.Kid, Default.AsymmetricSigningKey.KeyId },
                            { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType }
                        }.ToString(Formatting.None)
                    },
                    // Dates as integers
                    new ParseTimeValuesTheoryData
                    {
                        First = true,
                        Payload = new JObject()
                        {
                            { JwtRegisteredClaimNames.Email, "Bob@contoso.com" },
                            { JwtRegisteredClaimNames.GivenName, "Bob" },
                            { JwtRegisteredClaimNames.Iss, Default.Issuer },
                            { JwtRegisteredClaimNames.Aud, Default.Audience },
                            { JwtRegisteredClaimNames.Nbf, (float) EpochTime.GetIntDate(Default.NotBefore)},
                            { JwtRegisteredClaimNames.Exp, (float) EpochTime.GetIntDate(Default.Expires) }
                        }.ToString(Formatting.None),
                        Header = new JObject
                        {
                            { JwtHeaderParameterNames.Alg, SecurityAlgorithms.Sha512  },
                            { JwtHeaderParameterNames.Kid, Default.AsymmetricSigningKey.KeyId },
                            { JwtHeaderParameterNames.Typ, JwtConstants.HeaderType }
                        }.ToString(Formatting.None)
                    },
                };
            }
        }
    }

    public class ParseTimeValuesTheoryData : TheoryDataBase
    {
        public string Payload { get; set; }

        public string Header { get; set; }
    }
}
