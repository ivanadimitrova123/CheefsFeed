import React, { useState, useEffect, useContext } from "react";
import { useParams, useNavigate } from "react-router-dom";
import axios from "../axios/axios"; 
import Navbar from "../components/navbar";
import { Button, Spinner } from "react-bootstrap";
import Favorites from "../assets/images/favorites.svg";
import timer from "../assets/images/Timer.png";
import forknknife from "../assets/images/ForkKnife.png";
import level from "../assets/images/levels.svg";
import flag from "../assets/images/flag-black-shape.svg";
import { Store } from "../Store";
import PopularRecipes from "../components/PopularRecipes";
import RecomendedRecipes from "../components/RecomendedRecipes";
import Comment from "../components/Comment";
import { toast } from "react-toastify";

const RecipeDetails = () => {
  const { state } = useContext(Store);
  const { userInfo } = state;
  const [currentDate, setCurrentDate] = useState("");
  const { id } = useParams();
  const navigate = useNavigate();
  const [recipe, setRecipe] = useState(null);
  const [recipeUserImage, setRecipeUserImage] = useState("");
  const [recipeGrade, setRecipeGrade] = useState("");
  //const [reviews, setReviews] = useState("");
  const [rerender, setRerender] = useState(false);
  const [commentText, setCommentText] = useState("");
  const [comments, setComments] = useState([]);
  const [popularRecipes, setPopularRecipes] = useState("");
  const [reportIsLoading, setReportIsLoading] = useState(false);
  const [saveIsLoading, setSaveIsLoading] = useState(false);
  const [deleteIsLoading, setDeleteIsLoading] = useState(false);
  //const [gradeIsLoading, setGradeIsLoading] = useState(false);
  const [addCommentLoading, setAddCommentLoading] = useState(false);
  //const [deleteCommentLoading, setDeleteCommentLoading] = useState(false);
  const baseUrl = window.location.origin;

  useEffect(() => {
    const headers = {
      Authorization: `Bearer ${userInfo.token}`,
    };

    axios
      .get(`recipes/${id}`, { headers })
      .then((response) => {
        setRecipe(response.data);
        setRecipeUserImage(response.data.user.profilePictureId
            ? `${axios.defaults.baseURL}image/${response.data.user.profilePictureId}`
            : `${baseUrl}/default.jpg`);
     })
      .catch((error) => {
        console.error("Error fetching recipe details:", error);
      });

    const getCurrentDate = () => {
      const date = new Date();
      const options = { year: "numeric", month: "long", day: "numeric" };
      return date.toLocaleDateString("en-US", options);
    };

    setCurrentDate(getCurrentDate());
  }, [userInfo, id, baseUrl, recipe]);

  useEffect(() => {
    const headers = {
      Authorization: `Bearer ${userInfo.token}`,
    };

    axios
      .get(`recipes/popular`, { headers })
      .then((response) => {
        setPopularRecipes(response.data);
      })
      .catch((error) => {
        console.error("Error fetching popular recipes:", error);
      });

    const getCurrentDate = () => {
      const date = new Date();
      const options = { year: "numeric", month: "long", day: "numeric" };
      return date.toLocaleDateString("en-US", options);
    };

    setCurrentDate(getCurrentDate());
  }, [userInfo, id]);

  useEffect(() => {
    if (recipe && userInfo.user.id !== recipe.user.id) {
      const headers = {
        Authorization: `Bearer ${userInfo.token}`,
      };
      axios
        .get(`usergrades?userId=${userInfo.user.id}&recipeId=${recipe.id}`, { headers })
        .then((response) => {
          setRecipeGrade(response.data.grade);
          //setReviews(response.data.reviews);
        })
        .catch((error) => {
          console.error("Error fetching current user:", error);
        });
    }
  }, [userInfo, recipe]);

  useEffect(() => {
    const headers = {
      Authorization: `Bearer ${userInfo.token}`,
    };
    if (recipe && recipe.id != null) {
      axios
        .get(`comments/${recipe.id}`, { headers })
        .then((response) => setComments(response.data))
        .catch((error) => {
          console.error("Error fetching comments:", error);
        });
    }
    if (rerender === true) setRerender(false);
  }, [rerender, userInfo, recipe]);

  const handleEdit = () => {
    navigate(`/recipeForm/${id}`);
  };

  const handleDelete = () => {
    const headers = {
      Authorization: `Bearer ${userInfo.token}`,
    };

    setDeleteIsLoading(true);

    axios
      .delete(`recipes/${id}`, { headers })
      .then(() => {
        setDeleteIsLoading(false);
        if (userInfo.user.role === "Admin") {
          navigate(`/admin/dashboard`);
        } else {
          navigate(`/userProfile/${userInfo.user.id}`);
        }
      })
      .catch((error) => {
        console.error("Error deleting recipe:", error);
        setDeleteIsLoading(false);
      });
  };

  const gradeRecipe = (grade) => {
    const headers = {
      "Content-Type": "multipart/form-data",
      Authorization: `Bearer ${userInfo.token}`,
    };

    //setGradeIsLoading(true);

    const formData = new FormData();
    formData.append("userId", userInfo.user.id);
    formData.append("recipeId", recipe.id);
    formData.append("grade", grade);

    axios
      .post(`usergrades`, formData, { headers })
      .then(() => {
        setRecipeGrade(grade);
        //setGradeIsLoading(false);
      })
      .catch((error) => {
        console.error("Error Grading recipe:", error);
        //setGradeIsLoading(false);
      });
  };

  //TODO: CRETAE utils.js file, in which you can add small helper functions from all of the components
  // You can create getHeaders function with token and shouldHaveContentType properties
  const saveRecipe = () => {
    const headers = {
      "Content-Type": "multipart/form-data",
      Authorization: `Bearer ${userInfo.token}`,
    };

    setSaveIsLoading(true);

    const formData = new FormData();
    formData.append("userId", userInfo.user.id);
    formData.append("recipeId", recipe.id);

    axios
      .post(`saverecipe`, formData, { headers })
      .then(() => {
        setSaveIsLoading(false);
        toast.success("Recipe saved");
      })
      .catch((error) => {
        console.error("Error saving recipe:", error);
        setSaveIsLoading(false);
      });
  };

  const addCommentHandler = (e) => {
    e.preventDefault();
    setAddCommentLoading(true);
    const headers = {
      "Content-Type": "multipart/form-data",
      Authorization: `Bearer ${userInfo.token}`,
    };

    const formData = new FormData();
    formData.append("UserId", userInfo.user.id);
    formData.append("RecipeId", recipe.id);
    formData.append("Content", commentText);

    axios
      .post(`comments`, formData, { headers })
      .then(() => {
        setAddCommentLoading(false);
        setRerender(true);
        setCommentText("");
      })
      .catch((error) => {
        setAddCommentLoading(false);
        console.error("Error adding comment:", error);
      });
  };

  const deleteCommentHandler = async (id) => {
    //setDeleteCommentLoading(true);

    const headers = {
      "Content-Type": "multipart/form-data",
      Authorization: `Bearer ${userInfo.token}`,
    };

    axios
      .delete(`comments/${id}`, { headers })
      .then(() => {
        //setDeleteCommentLoading(false);
        setRerender(true);
      })
      .catch((error) => {
        //setDeleteCommentLoading(false);
        console.error("Error deleting comment:", error);
      });
  };

  const reportRecipe = async () => {
    setReportIsLoading(true);
    const headers = {
      "Content-Type": "multipart/form-data",
      Authorization: `Bearer ${userInfo.user.id}`,
    };

    const formData = new FormData();
    formData.append("userId", userInfo.user.id);
    formData.append("recipeId", recipe.id);

    axios
      .post(`reportedrecipe`, formData, { headers })
      .then((response) => {
        toast.success(response.data);
        setReportIsLoading(false);
      })
      .catch((error) => {
        setReportIsLoading(false);
        console.error("Error reporting recipe:", error);
      });
  };

  return (
    <div className="container-fluid">
      <Navbar />
      <div className="container mb-4">
        {recipe ? (
          <div>
            <div className="info-details mt-4">
              <h1 className="recipe-name">{recipe.name}</h1>
              <div className="recipeBoxDetails">
                {/* TODO: CREATE one component for minDetails, and call it with a map function(orginize the information with list of objects with 3 properties) */}
                <div className="detailsMins">
                  <div className="createdBy">
                    <img
                      src={
                        recipeUserImage === null ||
                        recipeUserImage === undefined ||
                        recipeUserImage === "" ||
                        recipeUserImage ===
                          "http://recipes-backend-id80.onrender.com/api/image/"
                          ? `${baseUrl}/default.jpg`
                          : recipeUserImage
                      }
                      alt={recipe.user.username}
                      className="recipe-user-image"
                    />
                    <div className="usernameFlex">
                      <p className="recipe-by">
                        <b>{recipe.user.username}</b>
                      </p>
                      <small>{currentDate}</small>
                    </div>
                  </div>

                  <div className="prepTime">
                    <img src={timer} alt="clock"></img>

                    <div className="usernameFlex">
                      <p className="recipe-by">
                        <b>PREP TIME</b>
                      </p>
                      <small>{recipe.prep} Minutes</small>
                    </div>
                  </div>

                  <div className="cookTime">
                    <img src={timer} alt="clock"></img>
                    <div className="usernameFlex">
                      <p className="recipe-by">
                        <b>COOK TIME</b>
                      </p>
                      <small>{recipe.cook} Minutes</small>
                    </div>
                  </div>

                  <div className="levels">
                    <img
                      src={level}
                      style={{ width: "20px", height: "20px" }}
                      alt="level"
                    />
                    <div className="usernameFlex">
                      <p className="recipe-by">
                        <b>LEVEL</b>
                      </p>
                      <small>{recipe.level}</small>
                    </div>
                  </div>

                  <div className="servings">
                    <img src={forknknife} alt="fork and knife"></img>
                    <div className="usernameFlex">
                      <p className="recipe-by">
                        <b>SERVINGS</b>
                      </p>
                      <small>{recipe.yield} Servings</small>
                    </div>
                  </div>

                  <div className="final">
                    <img
                      src={flag}
                      style={{ width: "20px", height: "20px" }}
                      alt="flag"
                    ></img>
                    <div className="usernameFlex">
                      <p className="recipe-by">
                        <b>TOTAL</b>
                      </p>
                      <small>{recipe.total} Minutes</small>
                    </div>
                  </div>
                </div>
                <div className="actionShareBtn">
                  <Button className="button-fav" onClick={saveRecipe}>
                    <img src={Favorites} alt="bookmark" />
                    {saveIsLoading ? (
                      <Spinner style={{ width: "1rem", height: "1rem" }} />
                    ) : (
                      <span>Save recipe</span>
                    )}
                  </Button>
                  <Button className="button-fav " onClick={reportRecipe}>
                    <img
                      src={flag}
                      alt="bookmark"
                      style={{ height: "1.5rem" }}
                    />
                    {reportIsLoading ? (
                      <Spinner style={{ width: "1rem", height: "1rem" }} />
                    ) : (
                      <span className="ms-2">Report recipe</span>
                    )}
                  </Button>
                </div>
              </div>
            </div>
            {/* Recipe rating */}
            <div className="numbered-timing mt-2">
              <div className="second-time newRating">
                <p>
                  Rating: <span>{parseFloat(recipe.rating).toFixed(2)}</span>
                </p>
                {Array.from({ length: 5 }, (_, index) => (
                  <span
                    key={index}
                    className={`star ${index < recipe.rating ? "filled" : ""}`}
                  >
                    &#9733;
                  </span>
                ))}
              </div>
            </div>
            <div className="mainSection mt-3">
              <div className="recipeNewImage">
                <img
                  src={
                    "data:" +
                    recipe.picture.contentType +
                    ";base64," +
                    recipe.picture.imageData
                  }
                  alt={recipe.name}
                ></img>
              </div>
              <PopularRecipes recipes={popularRecipes.slice(0, 3)} />
            </div>
            {/* Recipe description and ingredients */}
            {/* TODO: You do not need the second dif, just add the css in the first dif */}
            <div className="description-for-recipe mt-4">
              <div className="flex-items" style={{ display: "flex" }}>
                <div className="directions">
                  <h5>Directions</h5>
                  <p>{recipe.description}</p>
                </div>
                <div className="groupedClasses">
                  <div className="ingredients">
                    <h5>Ingredients:</h5>
                    <ul>
                      {recipe.ingredients.map((ingredient, index) => (
                        <li key={index}>{ingredient}</li>
                      ))}
                    </ul>
                  </div>
                </div>
              </div>
            </div>
            {/* User rating and save recipe button */}
            {userInfo.user.id !== recipe.user.id && (
              <div className="your-rating mt-4">
                <h5>Your rating:</h5>
                {Array.from({ length: 5 }, (_, index) => (
                  <span
                    key={index}
                    onClick={() => gradeRecipe(index + 1)}
                    style={{ cursor: "pointer" }}
                    className={`star ${index < recipeGrade ? "filled" : ""}`}
                  >
                    &#9733;
                  </span>
                ))}
              </div>
            )}
            {/* Recipe action buttons for the recipe owner */}
            {(userInfo.user.role === "Admin" ||
              userInfo.user.id === recipe.user.id) && (
              <div className="recipe-action-btns mt-4">
                <button className="btn-edit" onClick={handleEdit}>
                  Edit
                </button>
                <button className="btn-delete" onClick={handleDelete}>
                  {deleteIsLoading ? (
                    <Spinner style={{ width: "1rem", height: "1rem" }} />
                  ) : (
                    "Delete"
                  )}
                </button>
              </div>
            )}
            {/* TODO: comments part can be a separate component*/}
            {/* Comment section */}
            {userInfo.user.id !== recipe.user.id && (
              <>
                <h3 className="comment-heading mt-4">Comment</h3>
                <div className="comment-form">
                  <img
                    src={
                      userInfo.user.picture !== ""
                        ? userInfo.user.picture
                        : `${baseUrl}/default.jpg`
                    }
                    alt="profile"
                    className="comment-profile-image"
                  />
                  <form onSubmit={addCommentHandler}>
                    <textarea
                      type="text"
                      placeholder="Did you make this recipe? Leave a review!"
                      name="comment"
                      className="comment-input"
                      value={commentText}
                      onChange={(e) => setCommentText(e.target.value)}
                    />
                    <Button type="submit" className="post-review-btn">
                      {addCommentLoading ? (
                        <Spinner style={{ width: "1rem", height: "1rem" }} />
                      ) : (
                        "Post review"
                      )}
                    </Button>
                  </form>
                </div>
              </>
            )}
            {/* Display comments */}
            {comments && comments.length > 0 && (
              <>
                <h2 className="comments-heading mt-4">Comments</h2>
                <div className="commentsSection">
                  {comments &&
                    comments.map((c) => (
                      <Comment
                        comment={c}
                        key={c.commentId}
                        deleteHandler={deleteCommentHandler}
                      />
                    ))}
                </div>
              </>
            )}

            <RecomendedRecipes recipes={popularRecipes.slice(0,3)} />
          </div>
        ) : (
          <p className="loading-message mt-4">Loading recipe details...</p>
        )}
      </div>
    </div>
  );
};

export default RecipeDetails;
