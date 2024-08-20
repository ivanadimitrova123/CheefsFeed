import React, { useContext, useEffect, useState } from "react";
import axios from "../axios/axios";
import { Link, useNavigate } from "react-router-dom";
import chefImg from "../assets/images/chef.png";
import "../App.css";
import Navbar from "../components/navbar";
import DangerImg from "../assets/images/danger.png";
import { Store } from "../Store";

const Register = () => {
  const { state } = useContext(Store);
  const { userInfo } = state;

  const navigate = useNavigate();
  const [user, setUser] = useState({
    Username: "",
    Password: "",
    Email: "",
    FirstName: "",
    LastName: "",
  });

  const [loading, setloading] = useState(false);
  const [errorModal, setErrorModal] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const handleInputChange = (e) => {
    setUser({ ...user, [e.target.name]: e.target.value });
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      setloading(true);
      const response = await axios.post("account/register", user);
      setloading(false);
      navigate(`/login`);
      console.log(response);
    } catch (error) {
      setloading(false);
      console.error("Registration error", error);

      if (error.response && error.response.status === 400) {
        const { data } = error.response;
        if (data && (data.Email || data.Username)) {
          setErrorModal(true);

          let errorMessage = "";

          if (data.Email) {
            errorMessage += data.Email[0];
          }

          if (data.Username) {
            errorMessage += (errorMessage ? " " : "") + data.Username[0];
          }

          setErrorMessage(errorMessage);
        } else {
          setErrorMessage("Registration failed. Please try again.");
        }
      }
    }
  };

  useEffect(() => {
    console.log("userInfo in Register:", userInfo); // Debugging statement

    if (userInfo) {
      navigate("/feed");
    }
  }, [userInfo, navigate]);

  const closeModal = () => {
    setErrorModal(false);
  };

  return (
    <div className="container-fluid registerPage">
      <Navbar />
      <div className="row">
        <div className="col leftPart">
          <img src={chefImg} alt="" />
        </div>
        <div className="col">
          <div className="container-fluid registerForm">
            <h3 className="mb-2 text-center">
              <b>Register</b>
            </h3>
            {loading ? (
              <div className="text-center">
                <p>Loading...</p>
                <div className="spinner-border" role="status">
                  <span className="visually-hidden">Loading...</span>
                </div>
              </div>
            ) : (
              <form onSubmit={handleRegister}>
                <div className="mb-3">
                  <label htmlFor="FirstName" className="form-label">
                    <b>First Name:</b>
                  </label>
                  <input
                    type="text"
                    name="FirstName"
                    value={user.FirstName}
                    onChange={handleInputChange}
                    className="form-control"
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="LastName" className="form-label">
                    <b>Last Name:</b>
                  </label>
                  <input
                    type="text"
                    name="LastName"
                    value={user.LastName}
                    onChange={handleInputChange}
                    className="form-control"
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="Username" className="form-label">
                    <b>Username:</b>
                  </label>
                  <input
                    type="text"
                    name="Username"
                    value={user.Username}
                    onChange={handleInputChange}
                    className="form-control"
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="Email" className="form-label">
                    <b>Email:</b>
                  </label>
                  <input
                    type="email"
                    name="Email"
                    value={user.Email}
                    onChange={handleInputChange}
                    className="form-control"
                    required
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="Password" className="form-label">
                    <b>Password:</b>
                  </label>
                  <input
                    type="password"
                    name="Password"
                    value={user.Password}
                    onChange={handleInputChange}
                    className="form-control"
                    required
                  />
                </div>
                <div className="buttonSide">
                  <button
                    type="submit"
                    className="btn btn-primary rounded-pill"
                  >
                    <b>Register</b>
                  </button>
                </div>
              </form>
            )}
            <p className="mt-3 text-center">
              Already have an account{" "}
              <Link to={"/login"} className="">
                <b>Login</b>
              </Link>
            </p>
          </div>
        </div>
      </div>

      {errorModal && (
        <div
          className="modal"
          tabIndex="-1"
          role="dialog"
          style={{ display: "block" }}
        >
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  <b>Error</b> <img src={DangerImg} alt="" />
                </h5>
                <button
                  type="button"
                  className="close"
                  aria-label="Close"
                  onClick={closeModal}
                >
                  <span aria-hidden="true">
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      viewBox="0 0 50 50"
                      width="100px"
                      height="100px"
                    >
                      <path d="M 9.15625 6.3125 L 6.3125 9.15625 L 22.15625 25 L 6.21875 40.96875 L 9.03125 43.78125 L 25 27.84375 L 40.9375 43.78125 L 43.78125 40.9375 L 27.84375 25 L 43.6875 9.15625 L 40.84375 6.3125 L 25 22.15625 Z" />
                    </svg>
                  </span>
                </button>
              </div>
              <div className="modal-body">
                <p>
                  <b>{errorMessage}</b>
                </p>
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-secondary rounded-pill"
                  onClick={closeModal}
                >
                  OK
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Register;