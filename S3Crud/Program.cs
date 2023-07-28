using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace S3Crud
{
    public class Program
    {
        const string bucketName = "teste-crud";

        public static async Task Main()
        {
            await Crud();
        }

        private static async Task Crud()
        {
            Console.WriteLine("Escolha um serviço:");
            var result = Console.ReadLine();

            switch (result)
            {
                case "Atualizar":
                    Console.WriteLine("Digite o valor atualizado:");
                    var atualiza = Console.ReadLine();
                    await AdicionarObjeto(atualiza);
                    break;
                case "Criar":
                    Console.WriteLine("Digite o valor:");
                    var valor = Console.ReadLine();
                    await AdicionarObjeto(valor);
                    break;
                case "Deletar":
                    Console.WriteLine("Digite o valor atualizado:");
                    var key = Console.ReadLine();
                    await DeletarObjeto(key);
                    break;
                case "Obter":
                    Console.WriteLine("Digite o valor atualizado:");
                    var keyObter = Console.ReadLine();
                    await ObterObjeto(keyObter);
                    break;
                default:
                    Console.WriteLine("Terminou!");
                    break;
            }
        }

        //Se a key for igual ele atualiza, senão ele cria um novo
        public static async Task<PutObjectResponse> AdicionarObjeto(string? valor) 
        {
            var request = new PutObjectRequest { BucketName = bucketName, Key = "Hello World S3 Storage!", ContentBody = valor };
            var client = CriarClienteS3();

            try
            {
                PutObjectResponse response = await client.PutObjectAsync(request);
                return response;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);

                return new PutObjectResponse { HttpStatusCode = System.Net.HttpStatusCode.BadRequest};
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
                return new PutObjectResponse { HttpStatusCode = System.Net.HttpStatusCode.BadRequest };
            }

        }

        //Deleta objeto do bucket
        public static async Task<DeleteObjectResponse> DeletarObjeto(string? key)
        {
            var request = new DeleteObjectRequest { BucketName = bucketName, Key = key};
            var client = CriarClienteS3();

            try
            {
                var response = await client.DeleteObjectAsync(request);
                return response;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);

                return new DeleteObjectResponse { HttpStatusCode = System.Net.HttpStatusCode.BadRequest };
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
                return new DeleteObjectResponse { HttpStatusCode = System.Net.HttpStatusCode.BadRequest };
            }

        }

        //Obter objeto
        public static async Task<string> ObterObjeto(string? key)
        {
            var request = new GetObjectRequest { BucketName = bucketName, Key = key };
            var client = CriarClienteS3();
            string responseBody = "";

            try
            {
                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string contentType = response.Headers["Content-Type"];
                    Console.WriteLine("Content type: {0}", contentType);

                    responseBody = reader.ReadToEnd();
                }

                return responseBody;
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine(
                        "Error encountered ***. Message:'{0}' when writing an object"
                        , e.Message);

                return System.Net.HttpStatusCode.BadRequest.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    "Unknown encountered on server. Message:'{0}' when writing an object"
                    , e.Message);
                return System.Net.HttpStatusCode.BadRequest.ToString();
            }

        }

        private static AmazonS3Client CriarClienteS3()
        {
            var awsCredentials = new BasicAWSCredentials("access-key-id", "secret-key");
            var region = Amazon.RegionEndpoint.GetBySystemName("your-region");

            var s3Client = new AmazonS3Client(awsCredentials, region);

            return s3Client;
        }
    }
}