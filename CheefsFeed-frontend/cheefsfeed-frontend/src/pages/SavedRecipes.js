import React, { useContext, useEffect, useState, useRef } from "react";
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
    if (categoryId === 'all') {
      const response = await axios.get(`saverecipe?userId=${userInfo.user.id}`, { headers });
      setRecipes(response.data);
      setSelectedCategory(null); 
    } else {
      try {
        const response = await axios.get(`saverecipe/recipeByCategory?categoryId=${categoryId}`, { headers });
        setRecipes(response.data);
        setSelectedCategory(categoryId); 

        if (response.data.length === 0) {
          setErrorMessage("No recipes to show for this category.");
        }
      } catch (error) {
        console.error("Error fetching recipes by category:", error);
      }
    }

    setIsLoading(false);
  };

  const [currentIndex, setCurrentIndex] = useState(0);
  const categoryItemsRef = useRef(null);

  const scrollToCategory = (index) => {
    const categoryButtons = categoryItemsRef.current?.children;
    if (categoryButtons && categoryButtons[index]) {
      categoryButtons[index].scrollIntoView({ behavior: "smooth", inline: "center" });
    }
  };

  const handleNext = () => {
    if (currentIndex < categories.length - 1) {
      const newIndex = currentIndex + 1;
      setCurrentIndex(newIndex);
      scrollToCategory(newIndex);
      handleCategoryClick(categories[newIndex].id);
    }
  };

  const handlePrevious = () => {
    if (currentIndex > 0) {
      const newIndex = currentIndex - 1;
      setCurrentIndex(newIndex);
      scrollToCategory(newIndex);
      handleCategoryClick(categories[newIndex].id);
    }
  };

  return (
    <div className="container-fluid customBg" style={{ minHeight: "100vh" }}>
      <Navbar />
      <div className="row mb-3">
        <div className="col">
          <div className="categoryNav my-5">
            <button className="arrowButton" onClick={handlePrevious}>
              <svg className="rotated" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"><path d="M8.122 24l-4.122-4 8-8-8-8 4.122-4 11.878 12z" /></svg>
            </button>
            <div className="categoryItems" ref={categoryItemsRef}>
              <button
                key="all"
                className={`list-group-item ${selectedCategory === null ? 'active' : ''}`}
                onClick={() => handleCategoryClick('all')}
                style={{ cursor: "pointer" }}
              >
                All
              </button>
              {categories.map((category, index) => (
                <button
                  key={category.id}
                  className={`list-group-item ${selectedCategory === category.id ? 'active' : ''}`}
                  onClick={() => {
                    setCurrentIndex(index);
                    handleCategoryClick(category.id);
                  }}
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
