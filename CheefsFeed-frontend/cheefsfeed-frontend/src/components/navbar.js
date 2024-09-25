import React, { useState, useEffect, useContext } from "react";
import { Link, useNavigate } from "react-router-dom";
import ChefsFeedImage from "../assets/images/ChefsFeed.png";
import NavDropdown from "react-bootstrap/NavDropdown";
import "bootstrap/dist/css/bootstrap.min.css";
import { Store } from "../Store";
import axios from "../axios/axios";
import { getHeaders } from "../utils";
import '../App.css';

const Navbar = () => {
  const { state, dispatch: ctxDispatch } = useContext(Store);
  const { userInfo } = state;
  const navigate = useNavigate();
  const baseUrl = window.location.origin;
  const [searchText, setSearchText] = useState("");
  const [foundUsers, setFoundUsers] = useState([]);
  const [foundRecipes, setFoundRecipes] = useState([]);

  useEffect(() => {
    if (searchText.length > 0) {
      const headers = getHeaders(userInfo.token, false);

      axios
        .get(`account/search?text=${searchText}`, { headers })
        .then((response) => {
          setFoundUsers(response.data);
        })
        .catch((error) => {
          console.error("Error fetching searched users:", error);
        });

      axios
        .get(`recipes/search?text=${searchText}`, { headers })
        .then((response) => {
          setFoundRecipes(response.data);
        })
        .catch((error) => {
          console.error("Error fetching searched recipes:", error);
        });
    }
  }, [userInfo, searchText]);

  const signoutHandler = () => {
    ctxDispatch({ type: "USER_SIGNOUT" });
    localStorage.removeItem("userInfo");
    navigate("/login");
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light sticky-top">
      <div className="container-fluid">
        <Link className="navbar-brand" to="/feed">
          <img src={ChefsFeedImage} alt="Logo" height="30" />
        </Link>
        {userInfo && (
          <>
            <button
              className="navbar-toggler"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#navbarNav"
              aria-controls="navbarNav"
              aria-expanded="false"
              aria-label="Toggle navigation"
            >
              <span className="navbar-toggler-icon"></span>
            </button>
            <div className="collapse navbar-collapse" id="navbarNav">
              <ul className="navbar-nav ms-auto">
                <div className="position-relative me-2">
                  <input
                    className="searchNav form-control me-2"
                    type="search"
                    value={searchText}
                    onChange={(e) => setSearchText(e.target.value)}
                    placeholder="Search..."
                  />
                  {searchText.length > 0 && (
                    <div className="dropdown-menu show searchItemsList ps-1">
                      {/* Render found users */}
                      {foundUsers.length > 0 && "Users"}
                      {foundUsers.map((user) => (
                        <span
                          className="dropdown-item"
                          onClick={() => {
                            navigate(`/userProfile/${user.id}`);
                          }}
                          key={user.id}
                        >
                          <div
                            style={{ overflow: "hidden", cursor: "pointer" }}
                          >
                            <img
                              src={
                                user.picture !== ""
                                  ? user.picture
                                  : `${baseUrl}/default.jpg`
                              }
                              alt="profile-pic"
                              style={{ width: "30px", height: "30px" }}
                            />{" "}
                            {user.username}
                          </div>
                        </span>
                      ))}
                      {/* Render found recipes */}
                      {foundRecipes.length > 0 && "Recipes"}
                      {foundRecipes.map((recipe) => (
                        <span
                          className="dropdown-item"
                          onClick={() =>
                            navigate(`/recipeDetails/${recipe.id}`)
                          }
                          //href={`/recipeDetails/${recipe.id}`}
                          key={recipe.id}
                        >
                          <div
                            style={{ overflow: "hidden", cursor: "pointer" }}
                          >
                            <img
                              src={recipe.picture}
                              alt="profile-pic"
                              style={{ width: "30px", height: "30px" }}
                            />{" "}
                            {recipe.name}
                          </div>
                        </span>
                      ))}
                    </div>
                  )}
                </div>

                <li className="nav-item me-2">
                  <Link to="/feed" className="nav-link">
                    Feed
                  </Link>
                </li>
                <li className="nav-item">
                  <Link to="/savedRecipes" className="nav-link">
                    Saved Recipes
                  </Link>
                </li>
                <li className="nav-item">
                  <Link to="/followingList" className="nav-link">
                    Following List
                  </Link>
                </li>
                <li className="nav-item me-2">
                  <Link to="/recipeForm" className="nav-link">
                    Add Recipe
                  </Link>
                </li>
                <li className="nav-item">
                  <div className="currentLogged">
                    <NavDropdown
                      id="basic-nav-dropdown"
                      title={
                        <img
                          src={
                            userInfo && userInfo.user.picture !== ""
                              ? `${userInfo.user.picture}`
                              : `${baseUrl}/default.jpg`
                          }
                          alt="profile"
                        />
                      }
                    >
                      <NavDropdown.Item
                        onClick={() => {
                          navigate(`/userProfile/${userInfo.user.id}`);
                        }}
                      >
                        Profile
                      </NavDropdown.Item>
                      {userInfo.user.role === "Admin" && (
                        <NavDropdown.Item
                          onClick={() => {
                            navigate(`/admin/dashboard`);
                          }}
                        >
                          Dashboard
                        </NavDropdown.Item>
                      )}

                      <NavDropdown.Divider />
                      <NavDropdown.Item onClick={signoutHandler}>
                        Logout
                      </NavDropdown.Item>
                    </NavDropdown>
                  </div>
                </li>
              </ul>
            </div>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;