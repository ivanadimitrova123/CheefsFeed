import React, { useContext, useEffect, useState } from "react";
import axios from "../axios/axios"; 
import Navbar from "../components/navbar";
import { Store } from "../Store";
import FeedItem from "../components/FeedItem";
import { getHeaders } from "../utils";

const SavedRecipes = () => {
  const { state } = useContext(Store);
  const { userInfo } = state;
  const [recipes, setRecipes] = useState([]);
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');

  useEffect(() => {
    const fetchRecipes = async () => {
      setIsLoading(true);
      const headers = getHeaders(userInfo.token, false);

      try {
        const response = await axios.get(
          `saverecipe?userId=${userInfo.user.id}`,
          { headers }
        );
        setRecipes(response.data);
      } catch (error) {
        console.error("Error fetching saved recipes:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchRecipes();
  }, [userInfo]);

  useEffect(() => {
    const fetchCategories = async () => {
      const headers = getHeaders(userInfo.token, false);
      try {
        const response = await axios.get("category/getAllCategories", { headers });
        setCategories(response.data);
      } catch (error) {
        console.error("Error fetching categories:", error);
      }
    };

    fetchCategories();
  }, [userInfo]);

  const handleCategoryClick = async (categoryId) => {
    setIsLoading(true);
    const headers = getHeaders(userInfo.token, false);
    setErrorMessage(''); 

    try {
      const response = await axios.get(`saverecipe/recipeByCategory?categoryId=${categoryId}`, { headers });
      setRecipes(response.data);
      setSelectedCategory(categoryId); 

      if (response.data.length === 0) {
        setErrorMessage("No recipes to show for this category.");
      }
    } catch (error) {
      console.error("Error fetching recipes by category:", error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="container-fluid customBg" style={{ minHeight: "100vh" }}>
      <Navbar />
      <div className="row mb-3">
        <div className="col">
          <h3 className="mainHeader">Categories</h3>
          <ul className="list-group">
            {categories.map((category) => (
              <li
                key={category.id}
                className={`list-group-item ${selectedCategory === category.id ? 'active' : ''}`}
                onClick={() => handleCategoryClick(category.id)}
                style={{ cursor: "pointer" }}
              >
                {category.name}
              </li>
            ))}
          </ul>
        </div>
      </div>
      <div className="row mb-3">
        <div className="col" style={{ display: "flex", alignItems: "center" }}>
          <h2 className="mainHeader" style={{ width: "100%", textAlign: "center" }}>
            {selectedCategory ? `Recipes for Category` : `Saved Recipes`}
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
      {errorMessage && <p className="errorMessage mt-4 fs-5">{errorMessage}</p>}
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
        selectedCategory && !errorMessage && <p className="errorMessage mt-4 fs-5">No recipes found.</p>
      )}
    </div>
  );
};

export default SavedRecipes;
