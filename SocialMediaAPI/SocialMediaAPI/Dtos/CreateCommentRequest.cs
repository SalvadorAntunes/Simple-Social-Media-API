namespace SocialMediaAPI.Dtos
{
    public class CreateCommentRequest
    {
        public CreatePostRequest PostInfo { get; set; }

        public int PostCommented { get; set; }

    }
}
