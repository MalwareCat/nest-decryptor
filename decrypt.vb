Imports System.Text
Imports System.Security.Cryptography

Namespace MalwareCat
    Public Module Program
        Public Sub Main(args() As string)
            Console.WriteLine(Utils.DecryptString("fTEzAfYDoz1YzkqhQkH6GQFYKp1XY5hm7bjOP86yYxE="))
        End Sub
        
        Public Class Utils

            Public Shared Function DecryptString(EncryptedString As String) As String
                If String.IsNullOrEmpty(EncryptedString) Then
                    Return String.Empty
                Else
                    Return Decrypt(EncryptedString, "N3st22", "88552299", 2, "464R5DFA5DL6LE28", 256)
                End If
            End Function


            Public Shared Function Decrypt(ByVal cipherText As String, ByVal passPhrase As String, ByVal saltValue As String, ByVal passwordIterations As Integer, ByVal initVector As String, ByVal keySize As Integer) As String

                Dim initVectorBytes As Byte()
                initVectorBytes = Encoding.ASCII.GetBytes(initVector)

                Dim saltValueBytes As Byte()
                saltValueBytes = Encoding.ASCII.GetBytes(saltValue)

                Dim cipherTextBytes As Byte()
                cipherTextBytes = Convert.FromBase64String(cipherText)

                Dim password As New Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations)


                Dim keyBytes As Byte()
                keyBytes = password.GetBytes(CInt(keySize / 8))

                Dim symmetricKey As New AesCryptoServiceProvider
                symmetricKey.Mode = CipherMode.CBC

                Dim decryptor As ICryptoTransform
                decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)

                Dim memoryStream As IO.MemoryStream
                memoryStream = New IO.MemoryStream(cipherTextBytes)

                Dim cryptoStream As CryptoStream
                cryptoStream = New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)

                Dim plainTextBytes As Byte()
                ReDim plainTextBytes(cipherTextBytes.Length)

                Dim decryptedByteCount As Integer
                decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)


                memoryStream.Close()
                cryptoStream.Close()

                Dim plainText As String
                plainText = Encoding.ASCII.GetString(plainTextBytes, 0, decryptedByteCount)


                Return plainText
            End Function
        End Class
        
        
    End Module
End Namespace
