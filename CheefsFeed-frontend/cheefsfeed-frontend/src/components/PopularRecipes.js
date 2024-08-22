import React from "react";
import { useNavigate } from "react-router-dom";

const PopularRecipes = ({ recipes }) => {
  const navigate = useNavigate();
  return (
    <div className="recommendedBox">
      <h3>Popular Recipe</h3>
      <ul>
        {recipes &&
          recipes.map((recipe) => (
            <li
              key={recipe.id}
              style={{
                width: "100%",
                display: "flex",
                justifyContent: "start",
                padding: "10px",
              }}
              onClick={() => navigate(`/recipeDetails/${recipe.id}`)}
            >
              <img
                src={recipe.img}
                alt="food"
                style={{ width: "7rem", height: "7rem" }}
              ></img>
              <span>
                <h5>{recipe.name}</h5>
                <p>Recipe by {recipe.username}</p>
              </span>
            </li>
          ))}
      </ul>
    </div>
  );
};

export default PopularRecipes;