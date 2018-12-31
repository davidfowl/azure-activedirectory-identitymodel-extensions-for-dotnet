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

namespace Microsoft.IdentityModel.Tokens.Saml2
{
    /// <summary>
    /// This exception is thrown when SAML2 assertion decryption failed.
    /// </summary>
    public class Saml2SecurityTokenEncryptedAssertionDecryptionException : Saml2SecurityTokenEncryptedAssertionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenEncryptedAssertionDecryptionException"/> class.
        /// </summary>
        public Saml2SecurityTokenEncryptedAssertionDecryptionException()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenEncryptedAssertionDecryptionException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        public Saml2SecurityTokenEncryptedAssertionDecryptionException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Saml2SecurityTokenEncryptedAssertionDecryptionException"/> class.
        /// </summary>
        /// <param name="message">Additional information to be included in the exception and displayed to user.</param>
        /// <param name="innerException">A <see cref="Exception"/> that represents the root cause of the exception.</param>
        public Saml2SecurityTokenEncryptedAssertionDecryptionException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}