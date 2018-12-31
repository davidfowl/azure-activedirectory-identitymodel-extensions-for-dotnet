﻿//------------------------------------------------------------------------------
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

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Xml.Tests
{
    public class DataReferenceTests
    {
        [Fact]
        public void GetSets()
        {
            var type = typeof(DataReference);
            var properties = type.GetProperties();
            Assert.True(properties.Length == 1, $"Number of properties has changed from 1 to: {properties.Length}, adjust tests");

            var dataReference = new DataReference();
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("Uri", new List<object>{null, Guid.NewGuid().ToString()}),
                },
                Object = dataReference
            };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors($"{this}.GetSets", context.Errors);
        }

        [Theory, MemberData(nameof(ConstructorTheoryData))]
        public void Constructor(DataReferenceTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Constructor", theoryData);
            try
            {
                var dataReference = new DataReference(theoryData.Uri);
                IdentityComparer.AreEqual(dataReference.Uri, theoryData.Uri, context);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception exception)
            {
                theoryData.ExpectedException.ProcessException(exception, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DataReferenceTheoryData> ConstructorTheoryData()
        {
            return new TheoryData<DataReferenceTheoryData>
            {
                new DataReferenceTheoryData
                {
                    First = true,
                    Uri = null,
                    TestId = "NullUri"
                },
                new DataReferenceTheoryData
                {
                    Uri = "",
                    TestId = "EmptyUri"
                },
                new DataReferenceTheoryData
                {
                    Uri = Guid.NewGuid().ToString(),
                    TestId = "valid"
                },
            };
        }
    }

    public class DataReferenceTheoryData : TheoryDataBase
    {
        public string Uri { get; set; }
    }
}

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant
