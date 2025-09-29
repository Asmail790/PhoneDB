"use strict";

document.addEventListener("DOMContentLoaded", function () {
    const userReviewCard = document.getElementById("userReviewCardContainer")?.children[0] ?? null;
    const userReviewForm = document.getElementById("UserReviewForm")
    const button = document.getElementById("EditButton")


    const isReviewCardDisplayed = () => {
        return userReviewCard !== null && !userReviewCard.classList.contains("d-none")
    }

    const isReviewFormDisplayed = () => {
        return userReviewForm !== null && !userReviewForm.classList.contains("d-none")
    }

    const showReviewForm = () => {
        if (userReviewForm !== null) {
            userReviewForm.classList.remove("d-none")
        }
    }

    const showReviewCard = () => {
        if (userReviewCard !== null) {
            userReviewCard.classList.remove("d-none")
        }
    }


    const hideReviewCard = () => {
        if (userReviewCard !== null) {
            userReviewCard.classList.add("d-none");
        }
    };

    const hideReviewForm = () => {
        if (userReviewForm !== null) {
            userReviewForm.classList.add("d-none");
        }
    };


    if (button === null) {
        throw Error("element with id EditButton do not exist")
    }


    function setButtonLabel(text) {
        button.innerText = text;

    }


    showReviewCard();

    const userReviewExist = isReviewCardDisplayed();
    const editTextLabel = "Edit"
    const postTextLabel = "Add_Comment"
    const hideTextLabel = "Visibility_Off"

    const editOrPostTextLabel = userReviewExist ? editTextLabel : postTextLabel


    setButtonLabel(editOrPostTextLabel);


    button.addEventListener("click", function () {

        console.log(isReviewCardDisplayed())
        if (!isReviewFormDisplayed()) {
            setButtonLabel(hideTextLabel)
            hideReviewCard();
            showReviewForm();
        } else if (isReviewFormDisplayed()) {
            setButtonLabel(editOrPostTextLabel)
            hideReviewForm();
            showReviewCard();
        }

    })
});