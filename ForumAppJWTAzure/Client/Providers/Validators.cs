namespace ForumAppJWTAzure.Client.Providers
{
    public class UserProfileViewModelValidator : AbstractValidator<UserProfileViewModel>
    {
        public UserProfileViewModelValidator()
        {
            this.RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).When(x => x.ConfirmPassword?.Length >= 8 && x.Password?.Length >= 8).WithMessage("Password and Confirm Password do not match");
            this.RuleFor(x => x.Password).Equal(x => x.ConfirmPassword).When(x => x.ConfirmPassword?.Length >= 8 && x.Password?.Length >= 8).WithMessage("Password and Confirm Password do not match");
            this.RuleFor(x => x.ConfirmPassword).MinimumLength(8).When(x => x.ConfirmPassword?.Length > 0).WithMessage("The New Confirm Password field is required with a minimum length of 8 characters.");
            this.RuleFor(x => x.Password).MinimumLength(8).When(x => x.Password?.Length > 0 && x.ConfirmPassword?.Length > 0 && x.CurrentPassword?.Length > 0).WithMessage("The New Password field is required with a minimum length of 8 characters.");
            this.RuleFor(x => x.CurrentPassword).NotEmpty().MinimumLength(8).When(x => x.Password?.Length > 0 && x.ConfirmPassword?.Length > 0).WithMessage("The Current Password field is required with a minimum length of 8 characters.");
            this.RuleFor(x => x.Email).NotEmpty().EmailAddress();
            this.RuleFor(x => x.DisplayName).NotEmpty();
        }
    }

    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            this.RuleFor(x => x.ConfirmPassword).NotEmpty().Matches(y => y.Password).WithMessage("Password and confirm password do not match");
            this.RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            this.RuleFor(x => x.ConfirmPassword).MinimumLength(8);
            this.RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            this.RuleFor(x => x.DisplayName).NotEmpty();
        }
    }

    public class NewForumValidator : AbstractValidator<ForumViewModel>
    {
        public NewForumValidator()
        {
            this.RuleFor(x => x.Tags).NotEmpty().WithMessage("Please tag the forum with at least 1 tag");
            this.RuleFor(x => x.PostText).NotEmpty().WithMessage("Please enter some text for the post");
            this.RuleFor(x => x.Title).NotEmpty().WithMessage("Please create a title for the forum");
        }
    }
}
