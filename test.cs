 [HttpPost("/broker-api/SoapSend")]
 public async Task<ContentResult> SmsPost()
 {

     string phonePost = "";
     string textPost = "";
    

     #region get model

     try
     {
         string xmlString = "";

         using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
         {
             xmlString = reader.ReadToEnd();

         }
         
         if (xmlString != "")
         {
             _logger.LogInformation("Data: " + xmlString);
             xmlString= xmlString.Replace("ns0", "ns2");
             xmlString=xmlString.Replace("ns3", "ns2");
             using (StringReader reader = new StringReader(xmlString))
             {
                 //  var k = JsonConvert.DeserializeObject<Requests>(xmlString);
                 XmlDocument doc = new XmlDocument();
                 doc.LoadXml(xmlString);
                 string jsonText = JsonConvert.SerializeXmlNode(doc);
                 // XmlSerializer serializer = new XmlSerializer(typeof(Body));
                 var returnObject = JsonConvert.DeserializeObject<Root>(jsonText);
                 phonePost = returnObject.SoapEnvelope.SoapBody.Ns2SendSmsMessage.message.phone;
                 textPost = returnObject.SoapEnvelope.SoapBody.Ns2SendSmsMessage.message.text;

             }

         }
        
     }
     catch (Exception ex)
     {
         //log
         // return GetXml("post not format", 200);
         _logger.LogError("post not format");
         return GetXml("false", 200);
     }
     #endregion

     if (!IsPhoneFormat(phonePost))
     {
         //log
         //return GetXml("Phone not format 998xxxxxxxxx", 200);
         _logger.LogError("Phone not format 998xxxxxxxxx");
         return GetXml("false", 200);
     }



     RootObject model = new RootObject();
     Entity.Modals.Content content = new Entity.Modals.Content()
     {
         text = textPost.ToString()
     };
     Entity.Modals.Sms11 sms = new Entity.Modals.Sms11()
     {
         originator = "5800",
         content = content

     };

     Entity.Modals.Message message1 = new Entity.Modals.Message()
     {
         recipient = phonePost.ToString(),
         message_id = "uzcard" + RandomDigits(20),
         priority = "",
         sms = sms
     };
     List<Entity.Modals.Message> messages = new List<Entity.Modals.Message>();
     messages.Add(message1);
     model.messages = messages;


     if (model == null)
     {
         //log
         //return GetXml("model is null", 200);
         _logger.LogError("model is null");
         return GetXml("false", 200);

     }

     var provider = await GetProviderWithIP();

     if (provider == null)
     {

         //return GetXml("DELIVERED_TO_CLIENT", 200);
         _logger.LogError("provider is null");
         return GetXml("false", 200);

     }
     try
     {
         RootObject notExist = new RootObject() { messages = new List<Entity.Modals.Message>() };
         ParseRecipient(model);
         foreach (var i in model.messages)
         {
             AddSms(notExist, i);

         }
         if (notExist.messages.Count > 0)
         {
            await  _smsMessages.SaveDbMessage(notExist);
      
              return GetXml("true", 200);
              _logger.LogInformation("SendSoapSms: {0}", System.Text.Json.JsonSerializer.Serialize(notExist));
             
      
         }

     }
     catch (Exception ext)
     {
         _logger.LogError("Main:" + ext.Message);
         return GetXml("false", 200);
     }

     return GetXml("false", 200);

 }
