﻿namespace ForumAppJWTAzure.Server.Configurations
{
    public class MapperConfig : AutomapperProfile
    {
        public MapperConfig()
        {
            this.CreateMap<ApplicationUser, UserDto>().ReverseMap();
            this.CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
            this.CreateMap<BaseModel, BaseViewModel>().ReverseMap();

            this.CreateMap<Forum, ForumViewModel>();

            this.CreateMap<ForumViewModel, Forum>()
                .ForMember(q => q.CreatedBy, options => options.Ignore());

            this.CreateMap<Post, PostViewModel>();
            this.CreateMap<PostViewModel, Post>()
                .ForMember(x => x.CreatedBy, options => options.Ignore());

            this.CreateMap<Tag, TagViewModel>();
            this.CreateMap<TagViewModel, Tag>()
                .ForMember(x => x.Id, options => options.Ignore());

            this.CreateMap<VoteViewModel, Vote>().ReverseMap();

            this.CreateMap<AppLogViewModel, AppLog>().ReverseMap();

            this.CreateMap<FollowedForumViewModel, FollowedForum>().ReverseMap();

            this.CreateMap<NotificationViewModel, Notification>().ReverseMap();
        }
    }
}
