using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LoginApp.Models;

namespace LoginApp
{
    [Activity(Label = "AddUserActivity")]
    public class AddUserActivity : Activity
    {
        private EditText et_user;
        private EditText et_pass;
        private EditText et_info;
        private ImageView iv_close;
        private TextView tv_clear;
        private TextView tv_create;
        private TextView tv_allError;
        private TextView tv_userError;
        private TextView tv_passNumAndLetError;
        private TextView tv_passLengthError;
        private TextView tv_passRepeatError;
        bool finished = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_new_user);
            InitViews();
            InitViewLogic();
            finished = false;
        }

        private void InitViews()
        {
            et_user = FindViewById<EditText>(Resource.Id.et_user);
            et_pass = FindViewById<EditText>(Resource.Id.et_pass);
            et_info = FindViewById<EditText>(Resource.Id.et_info);
            iv_close = FindViewById<ImageView>(Resource.Id.iv_close);
            tv_clear = FindViewById<TextView>(Resource.Id.tv_clear);
            tv_create = FindViewById<TextView>(Resource.Id.tv_create);

            tv_allError = FindViewById<TextView>(Resource.Id.tv_allError);
            tv_userError = FindViewById<TextView>(Resource.Id.tv_userError);
            tv_passNumAndLetError = FindViewById<TextView>(Resource.Id.tv_passNumAndLetError);
            tv_passLengthError = FindViewById<TextView>(Resource.Id.tv_passLengthError);
            tv_passRepeatError = FindViewById<TextView>(Resource.Id.tv_passRepeatError);
        }

        private void InitViewLogic()
        {
            iv_close.Click += delegate {
                Finish();
            };

            tv_clear.Click += delegate
            {
                ClearInputs();
                HideErrors();
            };

            tv_create.Click += BtnCreate_Click;

            et_user.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                HideErrors();
            };

            et_pass.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                HideErrors();
            };

            et_info.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                HideErrors();
            };
        }

        private void ClearInputs()
        {
            et_user.Text = "";
            et_pass.Text = "";
            et_info.Text = "";
        }

        private void HideErrors()
        {
            tv_allError.Visibility = ViewStates.Invisible;
            tv_userError.Visibility = ViewStates.Invisible;
            tv_passNumAndLetError.Visibility = ViewStates.Invisible;
            tv_passLengthError.Visibility = ViewStates.Invisible;
            tv_passRepeatError.Visibility = ViewStates.Invisible;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {

            if (CheckFieldsComplete())
            {
                if (CheckNameNotTaken())
                {
                    if (!finished && CheckPasswordContainsOneLetterAndNumber())
                    {
                        CheckAll();
                    }
                    else
                    {
                        if (!finished) SetVisibility(1, tv_passNumAndLetError);
                    }

                    if (!finished && CheckLength())
                    {
                        CheckAll();
                    }
                    else
                    {
                        if (!finished) SetVisibility(1, tv_passLengthError);
                    }

                    if (!finished && CheckRepeat())
                    {
                        CheckAll();
                    }
                    else
                    {
                        if (!finished) SetVisibility(1, tv_passRepeatError);
                    }
                }
                else
                {
                    SetVisibility(1, tv_userError);
                }
            }
            else
            {
                SetVisibility(1, tv_allError);
            }
        }

        /**
         * TRUE: ALL FIELDS COMPLETE
         * FALSE: FIELDS INCOMPLETE
         * **/
        private bool CheckFieldsComplete()
        {
            if (et_user != null && et_pass != null)
            {
                return !(string.IsNullOrWhiteSpace(et_user.Text.ToString()) ||
                    string.IsNullOrWhiteSpace(et_pass.Text.ToString()));
            }
            else
                return false;
        }

        /**
         * TRUE: USERNAME DOESN'T EXIST IN USER TABLE
         * FALSE: USERNAME EXISTS
         * **/
        private bool CheckNameNotTaken()
        {
            return !(DBController.DoesUserNameExist(et_user.Text.ToString()));
        }

        /**
         * TRUE: PASSWORD CONTAINS AT LEAST 1 LETTER AND 1 NUMBER
         * FALSE: PASSWORD DOES NOT CONTAIN AT LEAST 1 LETTER AND 1 NUMBER
         * **/
        private bool CheckPasswordContainsOneLetterAndNumber()
        {
            bool containsSpace = et_pass.Text.ToString().Any(char.IsWhiteSpace);
            bool containsNumber = et_pass.Text.ToString().Any(char.IsDigit);
            bool containsLetter = et_pass.Text.ToString().Any(char.IsLetter);

            return (!containsSpace && containsNumber && containsLetter);
        }

        /**
         * TRUE: PASSWORD IS >= 5 CHARACTERS AND </= 12 CHARACTERS
         * FALSE: PASSWORD DOES NOT MEET LENGTH REQUIREMENTS
         * **/
        private bool CheckLength()
        {
            return (et_pass.Text.ToString().Length <= 12 && et_pass.Text.ToString().Length >= 5);
        }

        /**
         * TRUE: PASSWORD DOES NOT CONTAIN A REPEATING PATTERN
         * FALSE: PASSWORD REPEATS
         * **/
        private bool CheckRepeat()
        {
            string pass = et_pass.Text.ToString();

            for (int i = 0; i < pass.Length; i++)
            {
                char c1 = pass[i];
                int nextIndex = pass.IndexOf(c1, i + 1);

                while (nextIndex != -1)
                {
                    string s1 = pass.Substring(i, nextIndex - i);
                    if (2 * nextIndex - i <= pass.Length)
                    {
                        string s2 = pass.Substring(nextIndex, (2 * nextIndex - i) - nextIndex);
                        if (s1.Equals(s2))
                        {
                            return false;
                        }
                    }
                    nextIndex = pass.IndexOf(c1, nextIndex + 1);
                }

            }
            return true;
        }

        private void CheckAll()
        {
            if (CheckPasswordContainsOneLetterAndNumber() &&
                CheckLength() &&
                CheckRepeat())
            {
                finished = true;
                CreateNewUser();
            }
        }

        private void CreateNewUser()
        {
            DBController.InsertUser(new User {
                username = et_user.Text.ToString(),
                password = et_pass.Text.ToString(),
                info = et_info.Text.ToString()
            });
            Toast.MakeText(ApplicationContext, "Added User: " + et_user.Text.ToString(), ToastLength.Short).Show();
            Finish();
        }

        private void SetVisibility(int visibility, TextView error)
        {
            if (visibility == 1)
            {
                error.Visibility = ViewStates.Visible;
            }
            else if (visibility == 0)
            {
                error.Visibility = ViewStates.Invisible;
            }
            else
            {
                Console.WriteLine("SetVisibility error occurred.");
            }
        }
    }
}