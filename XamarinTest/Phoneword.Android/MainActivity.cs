using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using a_net = Android.Net;
using Android.Content.PM;
using Android.Runtime;

namespace Phoneword.Android
{
    [Activity(Label = "Phoneword", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        #region Constructors

        public MainActivity() : this(new PhoneTranslator())
        {
        }

        public MainActivity(IPhoneTranslator translator)
        {
            this.translator = translator;
            this.phoneCallPermissions = global::Android.Manifest.Permission.CallPhone;
        }

        #endregion

        #region Fields

        private const int phoneCallPermissionResult = 42;
        private readonly IPhoneTranslator translator;
        private readonly string phoneCallPermissions;

        // Child Controls
        private Button btnCall;
        private Button btnTranslate;
        private EditText txtPhoneword;

        private string phoneNumber;
        private bool callPhonePermissionGranted;
        private bool callPhonePermissionsAlreadyChecked;

        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            this.SetContentView(Resource.Layout.Main);

            this.GetControlsFromViewAndWireEvents();

            this.btnCall.Enabled = false;

            if (this.ApplicationContext.CheckSelfPermission(this.phoneCallPermissions) == Permission.Granted)
            {
                this.callPhonePermissionGranted = true;
                this.callPhonePermissionsAlreadyChecked = true;
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == MainActivity.phoneCallPermissionResult
                && (grantResults?.Length ?? 0) > 0
                && grantResults[0] == Permission.Granted)
            {
                this.callPhonePermissionGranted = true;
                this.callPhonePermissionsAlreadyChecked = true;

                this.BtnCall_Click(this, new EventArgs());

            }
            else
            {
                this.callPhonePermissionGranted = false;
                this.callPhonePermissionsAlreadyChecked = true;

                this.ShowCallPermissionDeniedDialog();
            }

        }

        private void GetControlsFromViewAndWireEvents()
        {
            this.txtPhoneword = this.FindViewById<EditText>(Resource.Id.txtPhoneword);

            this.btnTranslate = this.FindViewById<Button>(Resource.Id.btnTranslate);
            this.btnTranslate.Click += this.BtnTranslate_Click;

            this.btnCall = this.FindViewById<Button>(Resource.Id.btnCall);
            this.btnCall.Click += this.BtnCall_Click;
        }

        private void AskForPermission(string permission)
        {
            if (!this.callPhonePermissionsAlreadyChecked)
            {
                this.RequestPermissions(new string[] { this.phoneCallPermissions }, MainActivity.phoneCallPermissionResult);
            }
        }

        private void ShowStartCallDialog()
        {
            AlertDialog.Builder callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage($"Call {this.phoneNumber}?");
            callDialog.SetNeutralButton("Call", this.DialogButtonCall_Click);
            callDialog.SetNegativeButton("Cancel", this.DialogButtonCancel_Click);
            callDialog.Show();
        }

        private void ShowCallPermissionDeniedDialog()
        {
            AlertDialog.Builder callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage($"Permission to start a phone call has been denied!");
            callDialog.SetNegativeButton("Cancel", this.DialogButtonCancel_Click);
            callDialog.Show();
        }

        private void InitializePhoneCall()
        {
            Intent callPhoneNumber = new Intent(Intent.ActionCall);
            callPhoneNumber.SetData(a_net.Uri.Parse($"tel:{this.phoneNumber}"));
            this.StartActivity(callPhoneNumber);
        }

        #region Event Handlers

        private void BtnTranslate_Click(object sender, EventArgs e)
        {
            string userInput = this.txtPhoneword.Text;
            this.phoneNumber = this.translator.ToNumber(userInput);

            if (string.IsNullOrEmpty(this.phoneNumber))
            {
                this.btnCall.Text = "Call";
                this.btnCall.Enabled = false;
            }
            else
            {
                this.btnCall.Text = $"Call {this.phoneNumber}";
                this.btnCall.Enabled = true;
            }
        }

        private void BtnCall_Click(object sender, EventArgs e)
        {
            if (this.callPhonePermissionGranted)
            {
                this.ShowStartCallDialog();
            }
            else
            {
                if (!this.callPhonePermissionsAlreadyChecked)
                {
                    this.AskForPermission(this.phoneCallPermissions);
                }
            }
        }

        private void DialogButtonCall_Click(object sender, EventArgs e)
        {
            this.InitializePhoneCall();
        }

        private void DialogButtonCancel_Click(object sender, EventArgs e)
        {
            // Nothing to do
        }

        #endregion
    }
}

