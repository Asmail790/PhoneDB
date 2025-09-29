using DataBase;

public interface IReviewCard
{
    bool isUser { get; }
    PhoneReview PhoneReview { get; }
}


public class ReviewCard : IReviewCard
{
    public bool isUser { get; init; }
    public required PhoneReview PhoneReview { get; init; }
}