import React, { useState, useEffect, useContext } from "react";
import axios from "../axios/axios"; // Updated import
import Navbar from "../components/navbar";
import { Store } from "../Store";
import FeedItem from "../components/FeedItem";

const Feed = () => {
  const { state } = useContext(Store);
  const { userInfo } = state;
  const [recipes, setRecipes] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchRecipes = async () => {
      setIsLoading(true);
      const headers = {
        Authorization: `Bearer ${userInfo.token}`,
      };

      try {
        const response = await axios.get("follow/recipes", { headers });
        setRecipes(response.data);
      } catch (error) {
        console.error("Error fetching feed recipes:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchRecipes();
  }, [userInfo]);

  return (
    <div className="container-fluid customBg" style={{ minHeight: "100vh" }}>
      <Navbar />
      <div className="row mb-3">
        <div className="col" style={{ display: "flex", alignItems: "center" }}>
          <h2
            className="mainHeader"
            style={{ width: "100%", textAlign: "center" }}
          >
            Recipes Feed
          </h2>
        </div>
      </div>
      {isLoading && (
        <div className="text-center">
          <p>Loading...</p>
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
        </div>
      )}
      {recipes.length > 0 ? (
        <div className="row row-cols-1 row-cols-md-3 m-2 feedRow">
          {recipes.map((recipe) => (
            <FeedItem
              recipe={recipe.recipe}
              user={recipe.user}
              key={recipe.recipe.id}
            />
          ))}
        </div>
      ) : (
        <p className="errorMessage">No recipes from followed users.</p>
      )}
    </div>
  );
};

export default Feed;
