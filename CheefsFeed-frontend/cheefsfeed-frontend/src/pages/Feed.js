import React, { useState, useEffect, useContext, useRef } from "react";
import axios from "../axios/axios";
import Navbar from "../components/navbar";
import { Store } from "../Store";
import FeedItem from "../components/FeedItem";
import { getHeaders } from "../utils";

const Feed = () => {
  const { state } = useContext(Store);
  const { userInfo } = state;
  const [recipes, setRecipes] = useState([]);
  const [categories, setCategories] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState(null);

  useEffect(() => {
    const fetchRecipes = async () => {
      setIsLoading(true);
      const headers = getHeaders(userInfo.token, false);

      try {
        const response = await axios.get("follow/recipes", { headers });
        setRecipes(response.data);
      } catch (error) {
        console.error("Error fetching feed recipes:", error);
      } finally {
        setIsLoading(false);
      }
    };

    if (!selectedCategory) {
      fetchRecipes();
    }
  }, [userInfo, selectedCategory]);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get("category/getAllCategories");
        setCategories(response.data);
      } catch (error) {
        console.error("Error fetching categories:", error);
      }
    };

    fetchCategories();
  }, []);

  const handleCategoryClick = async (categoryId) => {
    setIsLoading(true);
    const headers = getHeaders(userInfo.token, false);

    try {
      const response = await axios.get(`follow/recipeByCategory?categoryId=${categoryId}`, { headers });
      setRecipes(response.data);
      setSelectedCategory(categoryId);
    } catch (error) {
      console.error("Error fetching recipes by category:", error);
    } finally {
      setIsLoading(false);
    }
  };

  const [currentIndex, setCurrentIndex] = useState(0);
  const categoryItemsRef = useRef(null);
  const scrollToCategory = (index) => {
    const categoryButtons = categoryItemsRef.current?.children;
    if (categoryButtons && categoryButtons[index]) {
      categoryButtons[index].scrollIntoView({ behavior: "smooth", inline: "center" });
      handleCategoryClick(categories[index].id);
    }
  };

  const handleNext = () => {
    if (currentIndex < categories.length - 1) {
      const newIndex = currentIndex + 1;
      setCurrentIndex(newIndex);
      scrollToCategory(newIndex);
    }
  };

  const handlePrevious = () => {
    if (currentIndex > 0) {
      const newIndex = currentIndex - 1;
      setCurrentIndex(newIndex);
      scrollToCategory(newIndex);
    }
  };

  return (
    <div className="container-fluid customBg" style={{ minHeight: "100vh" }}>
      <Navbar />

      {/* Display categories after the Navbar */}
      <div className="row mb-3">
        <div className="col">
          <h3>Categories</h3>
          <div className="categoryNav">
            <button className="arrowButton" onClick={handlePrevious}>
              <svg className="rotated" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8.122 24l-4.122-4 8-8-8-8 4.122-4 11.878 12z" /></svg>
            </button>
            <div className="categoryItems" ref={categoryItemsRef}>
              {categories.map((category) => (
                <button
                  key={category.id}
                  className={`list-group-item ${selectedCategory === category.id ? 'active' : ''}`}
                  onClick={() => handleCategoryClick(category.id)}
                  style={{ cursor: "pointer" }}
                >
                  {category.name}
                </button>
              ))}
            </div>
            <button className="arrowButton" onClick={handleNext}>
              <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8.122 24l-4.122-4 8-8-8-8 4.122-4 11.878 12z" /></svg>
            </button>
          </div>
        </div>
      </div>
      <div className="row mb-3">
        <div className="col" style={{ display: "flex", alignItems: "center" }}>
          <h2
            className="mainHeader"
            style={{ width: "100%", textAlign: "center" }}
          >
            {selectedCategory ? `Recipes for Category` : `Recipes Feed`}
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
        <p className="errorMessage">No recipes found.</p>
      )}
    </div>
  );
};

export default Feed;
