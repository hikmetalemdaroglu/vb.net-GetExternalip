' * **************************************************************************************** *
'    ip adress https://api.ipify.org , https://api6.ipify.org , https://api64.ipify.org
'              https://myexternalip.com/raw  (https://ipv4.myexternalip.com/raw ) ,
'              https://ipv6.myexternalip.com/raw   
' * **************************************************************************************** *
Imports System.IO
Imports System.Net
Imports System.Security.Policy
Imports System.Text
Imports GLib.IniFile
Imports Log.Log

Module Module1
    Sub Main()
        ' INI dosyasından değerleri okuyun
        Dim url As String = ReadIniValue("c:\settings.ini", "URL", "url")
        Dim logFilePath As String = ReadIniValue("c:\settings.ini", "Settings", "logFilePath")

        ' Dim url As String = "https://myexternalip.com/raw"
        ' Dim logFilePath As String = "c:\temp1\wanip.txt"
        Dim ipAddress As String
        url = readIni()
        Environment.Exit(0)
        Console.WriteLine("URL: " & url)
        Console.WriteLine("Log File Path: " & logFilePath)


        If Not Directory.Exists("c:\temp1") Then
            Directory.CreateDirectory("c:\temp1")
        End If
        ipAddress = GetPublicIpAddress(url)
        WriteToFile(logFilePath, ipAddress)
    End Sub
    Private Function GetPublicIpAddress(ByVal url As String) As String
        Dim request As HttpWebRequest
        Dim response As HttpWebResponse
        Dim ipAddress As String = ""
        Try
            request = CType(WebRequest.Create(url), HttpWebRequest)
            response = CType(request.GetResponse(), HttpWebResponse)
            If response.StatusCode = HttpStatusCode.OK Then
                Using receiveStream = response.GetResponseStream()
                    If receiveStream IsNot Nothing Then
                        Using readStream As StreamReader = New StreamReader(receiveStream, Encoding.UTF8)
                            ipAddress = readStream.ReadToEnd()
                        End Using
                    End If
                End Using
            End If
        Catch ex As Exception
            ipAddress = "Error: " + ex.Message
        End Try
        Return ipAddress
    End Function
    Private Sub WriteToFile(ByVal filePath As String, ByVal content As String)
        Dim file As New StreamWriter(filePath, False)
        file.WriteLine(content)
        file.Close()
    End Sub
    Function readIni() As String
        '-- Load TEST.INI file on current EXE directory
        ' D:\SourceVS\vb\TokenKonrol\TokenKonrol\bin\Debug
        Dim oIniFile As New GLib.IniFile
        Dim lLoad As Boolean = Nothing
        Dim vSystemStatu As String = Nothing
        Dim vUrl As String = ""
        lLoad = oIniFile.LoadFile("GetExternalip.ini")

        If (lLoad = False) Then
            '-- File not found
            MsgBox("ERROR 1 : GetExternalip.ini FILE Not FOUND")
        End If
        If (lLoad = True) Then
            vSystemStatu = oIniFile.Items("url")
            Console.Write("vSystemStatu")
            Console.ReadKey()
            Return vSystemStatu
            ' If vSystemStatu = "test" Then vurl = oIniFile.Items("TestUrl") Else vurl = oIniFile.Items("ProdUrl")
        End If
    End Function
    Function ReadIniValue(iniPath As String, section As String, key As String) As String
        '     ' INI dosyasından belirtilen bölüm ve anahtar değerini alın
        '     Dim result As String = ""
        '     Dim parser As New System.Configuration.IniFileParser()
        '     Dim iniData As System.Collections.Specialized.OrderedDictionary = parser.ReadIniFile(iniPath)
        ' 
        '     If iniData.Contains(section) Then
        '         Dim sectionData As System.Collections.Specialized.NameValueCollection = iniData(section)
        '         If sectionData.ContainsKey(key) Then
        '             result = sectionData.Get(key)
        '         End If
        '     End If
        ' 
        '     Return result
    End Function
End Module
