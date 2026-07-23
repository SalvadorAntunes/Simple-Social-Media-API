using System;
using System.Collections.Generic;

namespace SocialMediaAPI.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int PostCommented { get; set; }

    public virtual Post IdNavigation { get; set; } = null!;

    public virtual Post PostCommentedNavigation { get; set; } = null!;
}
