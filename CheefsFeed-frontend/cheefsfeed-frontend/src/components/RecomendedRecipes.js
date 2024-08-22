import React from "react";
import timer from "../assets/images/Timer.png";
import forknknife from "../assets/images/ForkKnife.png";
import { useNavigate } from "react-router-dom";

const RecomendedRecipes = ({ recipes }) => {
  const navigate = useNavigate();
  return (
    <>
      <h3>Recomended Recipes</h3>
      <div className="image-slider">
        {recipes &&
          recipes.map((recipe) => (
            <div
              key={recipe.id}
              className="cardItemFood"
              onClick={() => navigate(`/recipeDetails/${recipe.id}`)}
            >
              <img
                src={recipe.img}
                alt="food"
                style={{ width: "19rem", height: "15rem" }}
              ></img>
              <h5>{recipe.name}</h5>
              <span>
                <div>
                  <img src={timer} alt="timer"></img>
                  <p>{recipe.total}</p>
                </div>
                <div>
                  <img src={forknknife} alt="fork and knife"></img>
                  <p>{recipe.level}</p>
                </div>
              </span>
            </div>
          ))}
      </div>
    </>
  );
};

export default RecomendedRecipes;