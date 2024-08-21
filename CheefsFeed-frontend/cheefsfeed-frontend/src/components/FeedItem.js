import React from "react";
import { useNavigate } from "react-router-dom";

const FeedItem = ({ user, recipe }) => {
  const navigate = useNavigate();

  return (
    <div
      key={recipe.id}
      className="col-md-5 mb-2 card d-flex flex-column justify-content-between"
      style={{ height: "100%", position: "relative", cursor: "pointer" }}
      onClick={() => navigate(`/recipeDetails/${recipe.id}`)}
    >
      <div className="previewRecipe">
        <img
          src={
            user.userImage
              ? user.userImage
              : `${window.location.origin}/default.jpg` // Fallback URL if no userImage is provided
          }
          alt="Profile"
          className="img-fluid rounded-circle"
        />
        <p>{user.username}</p>
      </div>
      <div style={{ overflow: "hidden", height: "300px" }}>
        <img
          src={
            recipe.recipeImage
              ? recipe.recipeImage
              : `${window.location.origin}/default.jpg` // Fallback URL if no recipeImage is provided
          }
          alt="food"
          style={{
            maxWidth: "100%",
            maxHeight: "100%",
            objectFit: "cover",
            height: "300px",
            width: "100%",
          }}
        />
      </div>
      <p className="recipeName">{recipe.name}</p>

      <div className="recipeRating">
        {Array.from({ length: 5 }, (_, index) => (
          <div key={index}>
            <span
              key={index}
              className={`star ${index < recipe.rating ? "filled" : ""}`}
            >
              ★
            </span>
          </div>
        ))}

        <div className="comments-section">
          <p>{recipe.comments}</p>
        </div>
      </div>
    </div>
  );
};

export default FeedItem;
