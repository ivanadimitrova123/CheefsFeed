import React, { useContext, useEffect, useState } from "react";
import axios from "../axios/axios"; 
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import Navbar from "../components/navbar";
import { Store } from "../Store";
import { Spinner } from "react-bootstrap";

function RecipeForm() {
  const { state } = useContext(Store);
  const { userInfo } = state;
  const { id } = useParams();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [recipe, setRecipe] = useState({
    name: "",
    description: "",
    ingredients: "",
    level: "easy",
    prep: "",
    cook: "",
    total: "",
    yield: "",
    categoryId: "",
  });

  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState("");
  const [photo, setPhoto] = useState(null);

  // Fetch categories
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get("/category/getAllCategories", {
          headers: {
            Authorization: `Bearer ${userInfo.token}`,
          },
        });
        setCategories(response.data);
      } catch (error) {
        console.error("Error fetching categories:", error);
      }
    };

    fetchCategories();
  }, [userInfo]);

  useEffect(() => {
    if (id) {
      axios
        .get(`/recipes/${id}`, {
          headers: {
            Authorization: `Bearer ${userInfo.token}`,
          },
        })
        .then((response) => {
          const fetchedRecipe = response.data;
          fetchedRecipe.ingredients = fetchedRecipe.ingredients.join("\n");
          setRecipe(fetchedRecipe);
          setSelectedCategory(fetchedRecipe.categoryId || "");

          setPhoto(
            `data:${fetchedRecipe.picture.contentType};base64,${fetchedRecipe.picture.imageData}`
          );
        })
        .catch((error) => {
          console.error("Error fetching recipe data:", error);
        });
    }
  }, [userInfo, id]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setRecipe({
      ...recipe,
      [name]: value,
    });
  };

  const handleCategoryChange = (e) => {
    setSelectedCategory(e.target.value);
  };

  const handleFileChange = (e) => {
    setPhoto(e.target.files[0]);
  };

  const handleCreateOrUpdateRecipe = async (e) => {
    if (
      recipe.name === "" ||
      recipe.description === "" ||
      photo === null ||
      recipe.ingredients === ""
    ) {
      alert("No empty fields allowed");
    }
    e.preventDefault();
    setIsLoading(true);

    const formData = new FormData();
    formData.append("name", recipe.name);
    formData.append("description", recipe.description);
    formData.append("ingredients", [recipe.ingredients]);
    formData.append("level", recipe.level);
    formData.append("cook", recipe.cook);
    formData.append("prep", recipe.prep);
    formData.append("total", parseInt(recipe.cook || 0) + parseInt(recipe.prep || 0));
    formData.append("yield", recipe.yield);
    formData.append("photo", photo);
    formData.append("categoryId", selectedCategory);
    console.log(selectedCategory);
    try {
      if (id) {
        await axios.put(
          `/recipes/${id}`,
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
              Authorization: `Bearer ${userInfo.token}`,
            },
          }
        );

        setIsLoading(false);
        navigate(`/recipeDetails/${id}`);      
      } else {

        await axios.post(
            "/recipes",
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data",
              Authorization: `Bearer ${userInfo.token}`,
            },
          }
        );
        setIsLoading(false);
        navigate(`/userProfile/${userInfo.user.id}`);
      }
    } catch (error) {
      console.error(error);
      setIsLoading(false);
    }
  };

  return (
    <div className="container-fluid">
      <Navbar />
      <div className="container">
        <div className="form-group recipeHeader">
          <div
            className="recipePhotoCover d-flex flex-column align-items-center justify-content-center"
            style={{
              height: "100%",
              backgroundImage: `url(${photo || ""})`,
              backgroundSize: "cover",
              backgroundPosition: "center",
            }}
          >
            <label htmlFor="photo" className="fs-3 d-block">Add Recipe Photo</label>
            <input type="file" accept="image/*"
                onChange={handleFileChange}       
                className="mt-3 "
                style={{ fontSize: "1.11rem", marginLeft: "3rem" }}
            />
          </div>
        </div>
        <div className="form-group">
          <input
            type="text"
            name="name"
            className="form-control mt-4 recipeTitle fs-5"
            placeholder="Enter title of recepie"
            value={recipe.name}
            onChange={handleInputChange}
          />
        </div>
        <h3 className="mt-4 mb-4">{id ? "Edit Recipe" : "Create Recipe"}</h3>
        <form className="recipeDetailsForm">
          <div className="row">
            <div className="col-4">
              <div className="form-group">
                <label htmlFor="category" className="form-label fw-bold">Category:</label>
                <select
                  className="form-select"
                  name="category"
                  value={selectedCategory}
                  onChange={handleCategoryChange}
                >
                  <option value="">Select Category</option>
                  {categories.map((category) => (
                    <option key={category.id} value={category.id}>
                      {category.name}
                    </option>
                  ))}
                </select>
              </div>
            </div>
            <div className="col-4">
                <div className="form-group">
                    <label htmlFor="form-select" className="form-label fw-bold">Level:</label>
                    <select
                    className="form-select"
                    name="form-select"
                    aria-label="Select level"
                    value={recipe.level}
                    onChange={(e) => setRecipe({ ...recipe, level: e.target.value })}
                    >
                        <option value="easy">Easy</option>
                        <option value="medium">Medium</option>
                        <option value="hard">Hard</option>
                    </select>
                </div>
            </div>
            <div className="col-4">
                <div className="form-group">
                <label htmlFor="yield" className="form-label  fw-bold">Yield:</label>
                <input
                    type="number"
                    className="form-control"
                    name="yield"
                    value={recipe.yield}
                    onChange={handleInputChange}
                    id="yield"
                />
                </div>
            </div>
        </div>
        {/*TODO: change for medium devices */}
        <div className="row mt-4">
            <div className="col-md-4">
                <div className="form-group">
                <label htmlFor="prep" className="form-label fw-bold">Prep:</label>
                <div className="input-group">
                    <input
                    type="number"
                    className="form-control"
                    name="prep"
                    value={recipe.prep}
                    onChange={handleInputChange}
                    id="prep"
                    />
                    <span className="input-group-text">min</span>
                </div>
                </div>
            </div>
            <div className="col-4">
                <div className="form-group">
                <label htmlFor="cook" className="form-label fw-bold">Cook:</label>
                <div className="input-group">
                    <input
                    type="number"
                    className="form-control"
                    name="cook"
                    value={recipe.cook}
                    onChange={handleInputChange}
                    id="cook"
                    />
                    <span className="input-group-text">min</span>
                </div>
                </div>
            </div>
            <div className="col-4">
                <div className="form-group">
                <label htmlFor="total" className="form-label fw-bold">Total:</label>
                <div className="input-group">
                    <input
                    type="number"
                    className="form-control"
                    name="total"
                    value={parseInt(recipe.cook || 0) + parseInt(recipe.prep || 0)}
                    disabled
                    id="total"
                    />
                    <span className="input-group-text">min</span>
                </div>
                </div>
            </div>
        </div>

          <div className="ingredientAndDesc">
            <div className="form-group">
              <label htmlFor="description">Description:</label>
              <textarea
                name="description"
                className="form-control"
                value={recipe.description}
                onChange={handleInputChange}
              />
            </div>
            <div className="form-group">
              <label htmlFor="ingredients">Ingredients:</label>
              <textarea
                name="ingredients"
                className="form-control"
                value={recipe.ingredients}
                onChange={handleInputChange}
              />
            </div>
          </div>

          <button
            className="btn btn-danger mt-5"
            onClick={handleCreateOrUpdateRecipe}
          >
            {isLoading ? (
              <Spinner style={{ height: "1rem", width: "1rem" }} />
            ) : id ? (
              "Update Recipe"
            ) : (
              "Create Recipe"
            )}
            {/* {id ? "Update Recipe" : "Create Recipe"} */}
          </button>
        </form>
      </div>
    </div>
  );
}

export default RecipeForm;