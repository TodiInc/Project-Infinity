using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseAuthManager : MonoBehaviour
{
    [Header("Firebase stuff")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public static FirebaseUser user;

    [Header("Login Fields")]
    public InputField emailLoginField;
    public InputField passwordLoginField;

    [Header("Registration Fields")]
    public InputField nameRegistrationField;
    public InputField emailRegistrationField;
    public InputField passwordRegistrationField;
    public InputField confirmPasswordRegistrationField;

    [Header("UI control")]
    public Canvas loginForm;
    public Canvas registerForm;

    [SerializeField] private static FirebaseAuthManager _Instance;

    public static FirebaseAuthManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<FirebaseAuthManager>();
                _Instance.name = _Instance.GetType().ToString();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }
    private void Awake (){

        DatabaseReference dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>{
            dependencyStatus = task.Result;

            if(dependencyStatus == DependencyStatus.Available){
                InitilizeFirebase();
            }
            else{
                Debug.LogError("Could not resolve all firebase dependencies" + dependencyStatus);
            }
        });
    }

    public void Update()
    {
        // if (Input.GetKeyDown("space")) continue when free
        // {
        //     FirebaseDatabase.DefaultInstance
        //     .GetReference("UserInventories")
        //     .GetValueAsync().ContinueWithOnMainThread(task => {

        //     if (task.IsFaulted) {
        //         Debug.Log("Could not get object");
        //     }
        //     else if (task.IsCompleted) {

        //         DataSnapshot snapshot = task.Result;
        //         Item item = ScriptableObject.CreateInstance<Item>();
        //         snapshot.Child(user.UserId).Child("items").Children.;
        //         item.setItemAttribute(iAP.ItemAttribute);
        //         item.setItemStats(iAP.Damage, iAP.Health);
        //         item.setItemDescription(iAP.ItemDescription);
        //         item.setItemIcon(iAP.ItemIcon);
        //         item.setItemType(iAP.ItemType);
        //         DataSnapshot itemSnapshot = snapshot.Child();
        //     }
        // });
        // }
    }

    private void InitilizeFirebase(){
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs){
        if(auth.CurrentUser != user){
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if(!signedIn && user != null){
                Debug.Log("Signed out" + user.UserId);
            }

            user = auth.CurrentUser;

            if(signedIn){
                Debug.Log("Signed in: "+user.UserId);
            }
        }
    }

    public void Login(){
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password){
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email,password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if(loginTask.Exception != null){
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;

            AuthError authError = (AuthError)firebaseException.ErrorCode;

            string failedMessage = "Login failed! Because ";

            switch (authError){
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password missing";
                    break;
                default:
                    failedMessage += "Login Failed";
                    break;
            }
            Debug.Log(failedMessage);
        }
        else{
            user = loginTask.Result;

            Debug.LogFormat("{0} You are successfully logged in", user.DisplayName);

            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }

    public void Register(){
        StartCoroutine(RegisterAsync(nameRegistrationField.text, emailRegistrationField.text, passwordRegistrationField.text, confirmPasswordRegistrationField.text));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword){
        if(name ==""){
            Debug.LogError("Username field is empty");
        }
        else if(email == ""){
            Debug.Log("email field is empty");
        }
        else if(passwordRegistrationField.text != confirmPasswordRegistrationField.text){
            Debug.LogError("Passwords do not match");
        }
        else {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email,password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if(registerTask.Exception != null){
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;

                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration failed! Because ";
                switch (authError){
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Password is wrong";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email field is empty";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password field is empty";
                        break;
                    default:
                        failedMessage += "Registration Failed";
                        break;
                }

                Debug.Log(failedMessage);
            }
            else{
                user = registerTask.Result;

                UserProfile userProfile = new UserProfile { DisplayName = name};

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if(updateProfileTask.Exception != null){
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;

                    AuthError authError = (AuthError)firebaseException.ErrorCode;

                    string failedMessage = "Update failed! Because ";
                    switch (authError){
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Password is wrong";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email field is empty";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password field is empty";
                            break;
                        default:
                            failedMessage += "Update Failed";
                            break;
                    }
                    Debug.Log(failedMessage);
                }
                else{
                    Debug.Log("Registration successful! Welcome: " + user.DisplayName);
                    loginForm.GetComponent<Canvas>().enabled = true;
                    registerForm.GetComponent<Canvas>().enabled = false;
                }
            }
        }
    }
}
