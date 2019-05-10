using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HelloWorld
{
    
    public class Function
    {
        IAmazonS3 S3Client { get; set; }

        public Function()
        {
            S3Client = new AmazonS3Client();
        }

        public Function(IAmazonS3 client)
        {
            S3Client = client;
        }

        /// <summary>
        /// A simple function that returns hello world
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(ILambdaContext context)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = "demo-app-file-bucket",
                Key = "uppercase.txt"
            };

            using (GetObjectResponse response = await S3Client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                string contentType = response.Headers["Content-Type"];
                Console.WriteLine("Object metadata, Title: {0}", title);
                Console.WriteLine("Content type: {0}", contentType);

                return reader.ReadToEnd(); // Now you process the response body.
            }
        }
    }
}
