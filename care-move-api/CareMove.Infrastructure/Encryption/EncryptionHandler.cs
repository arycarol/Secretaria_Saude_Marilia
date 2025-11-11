using System.Security.Cryptography;
using System.Text;

// Define o namespace da sua aplicação
namespace CareMove.Infrastructure.Encryption
{
    /// <summary>
    /// Fornece métodos simples para criptografar e descriptografar strings
    /// usando AES-256 com uma senha.
    /// </summary>
    public static class EncryptionHandler
    {
        // Tamanho da chave em bytes (AES-256)
        private const int KeySize = 32;
        // Tamanho do Vetor de Inicialização (IV) em bytes (AES usa blocos de 128 bits)
        private const int IvSize = 16;
        // Número de iterações para o PBKDF2 (quanto maior, mais seguro, mais lento)
        private const int Iterations = 600_000;

        // O "Salt" (Sal) deve ser único e secreto. 
        // Em uma aplicação real, você NUNCA deve hardcodar o Salt assim se
        // estiver armazenando senhas. 
        // Para este caso de uso (criptografar dados com uma senha mestra),
        // ele pode ser armazenado de forma segura na configuração da aplicação.
        // Para este EXEMPLO, vamos gerá-lo e hardcodá-lo.
        // Você pode gerar o seu próprio usando: RandomNumberGenerator.GetBytes(16)
        private static readonly byte[] Salt =
        {
            0x1A, 0x2B, 0x3C, 0x4D, 0x5E, 0x6F, 0x70, 0x81,
            0x92, 0xA3, 0xB4, 0xC5, 0xD6, 0xE7, 0xF8, 0x09
        };

        /// <summary>
        /// Criptografa uma string de texto puro usando uma senha.
        /// </summary>
        /// <param name="plainText">O texto a ser criptografado.</param>
        /// <returns>Uma string Base64 representando os dados criptografados (IV + Ciphertext).</returns>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(EncryptionKey.Key))
                throw new ArgumentNullException(nameof(EncryptionKey.Key));

            // 1. Derivar a chave da senha usando PBKDF2
            byte[] key = GetKey(EncryptionKey.Key);

            // 2. Converter o texto puro para bytes
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            // 3. Criar a instância do AES
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC; // Modo de operação
                aes.Padding = PaddingMode.PKCS7; // Modo de preenchimento

                // 4. Gerar um IV (Vetor de Inicialização) aleatório e seguro
                aes.GenerateIV();
                byte[] iv = aes.IV;

                // 5. Criar o criptografador
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // 6. Criptografar os dados
                byte[] cipherBytes;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                    } // O 'FlushFinalBlock' é chamado aqui quando o CryptoStream é fechado
                    cipherBytes = ms.ToArray();
                }

                // 7. Combinar o IV e os dados criptografados
                // Precisamos salvar o IV junto com os dados para podermos descriptografar
                byte[] result = new byte[iv.Length + cipherBytes.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(cipherBytes, 0, result, iv.Length, cipherBytes.Length);

                // 8. Retornar como uma string Base64
                return Convert.ToBase64String(result);
            }
        }

        /// <summary>
        /// Descriptografa uma string Base64 (criada pelo método Encrypt) usando a senha.
        /// </summary>
        /// <param name="cipherTextBase64">A string criptografada (IV + Ciphertext) em Base64.</param>
        /// <returns>A string de texto puro original.</returns>
        public static string Decrypt(string cipherTextBase64)
        {
            if (string.IsNullOrEmpty(cipherTextBase64))
                throw new ArgumentNullException(nameof(cipherTextBase64));
            if (string.IsNullOrEmpty(EncryptionKey.Key))
                throw new ArgumentNullException(nameof(EncryptionKey.Key));

            // 1. Derivar a chave da senha (deve ser idêntica à usada na criptografia)
            byte[] key = GetKey(EncryptionKey.Key);

            // 2. Converter a string Base64 de volta para bytes
            byte[] fullCipher = Convert.FromBase64String(cipherTextBase64);

            // 3. Extrair o IV e os dados criptografados do array de bytes
            if (fullCipher.Length < IvSize)
                throw new CryptographicException("Dados criptografados inválidos. O comprimento é menor que o IV.");

            byte[] iv = new byte[IvSize];
            byte[] cipherBytes = new byte[fullCipher.Length - IvSize];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

            // 4. Criar a instância do AES
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv; // Usar o IV que extraímos
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // 5. Criar o descriptografador
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                // 6. Descriptografar os dados
                byte[] plainBytes;
                try
                {
                    using (var ms = new MemoryStream(cipherBytes))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var reader = new StreamReader(cs, Encoding.UTF8))
                            {
                                // Ler os dados descriptografados
                                string plainText = reader.ReadToEnd();
                                // Se o padding estiver incorreto ou a chave errada,
                                // uma exceção CryptographicException será lançada aqui.
                                return plainText;
                            }
                        }
                    }
                }
                catch (CryptographicException ex)
                {
                    // Causa comum: Senha errada ou dados corrompidos.
                    throw new CryptographicException("Falha ao descriptografar. Senha pode estar incorreta ou os dados estão corrompidos.", ex);
                }
            }
        }

        /// <summary>
        /// Gera uma chave de 32 bytes (256 bits) a partir de uma senha
        /// usando PBKDF2 com SHA256.
        /// </summary>
        private static byte[] GetKey(string password)
        {
            // Rfc2898DeriveBytes implementa o PBKDF2
            // Usamos o Salt estático e o número de iterações definido
            return Rfc2898DeriveBytes.Pbkdf2(
                password: password,
                salt: Salt,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySize // 32 bytes = 256 bits
            );
        }
    }
}