using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Global;
using Google.Apis.Auth.OAuth2;
using SCommon;

namespace GateServer
{
    class CFirebaseManager : SSingleton<CFirebaseManager>
    {
        private FirebaseApp m_App;

        public void Init()
        {
            m_App = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(@".\Config\knightidle-da3ca-firebase-adminsdk-dl7sh-1510de3ad1.json")
            });
        }

        public async Task VerifyIDToken(long sessionKey, int serverKey, string idToken)
        {
            try
            {
                FirebaseToken token = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                string deviceID = token.Uid;
                CDBManager.Instance.QueryAccountAuth(sessionKey, serverKey, deviceID, CDefine.AuthType.Firebase);
            }
            catch (FirebaseAuthException ex)
            {
                //error
                Console.WriteLine($"FirebaseAuthException: {ex.Message}");
            }
            catch (Exception ex)
            {
                //error
                Console.WriteLine($"Exception: {ex.Message}");
                // 일반 예외 처리
            }
        }
    }
}
