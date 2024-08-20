import React, { useContext, useEffect, useState } from "react";
import axios from "../axios/axios";
import { Link, useNavigate } from "react-router-dom";
import Navbar from "../components/navbar";
import DangerImg from "../assets/images/danger.png";
import { Store } from "../Store";
import "../App.css";

const Login = () => {
  const { state, dispatch: ctxDispatch } = useContext(Store);
  const { userInfo } = state;
  const navigate = useNavigate();
  const [loginInfo, setLoginInfo] = useState({
    Username: "",
    Password: "",
  });

  const [loading, setLoading] = useState(false);
  const [errorModal, setErrorModal] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const handleInputChange = (e) => {
    setLoginInfo({ ...loginInfo, [e.target.name]: e.target.value });
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      const response = await axios.post("account/login", loginInfo);
      if (response.status === 200) {
        ctxDispatch({ type: "USER_SIGNIN", payload: response.data });
        localStorage.setItem("userInfo", JSON.stringify(response.data));
        navigate(`/feed`);
      }
    } catch (error) {
      setLoading(false);

      if (error.response && error.response.status === 400) {
        setErrorModal(true);
        setErrorMessage("Incorrect username or password. Please try again.");
        console.error(errorMessage);

      }
    }
  };

  const closeModal = () => {
    setErrorModal(false);
  };

  useEffect(() => {
    if (userInfo) {
      navigate("/feed");
    }
  });

  return (
    <div className="container-fluid loginPageBg">
      <Navbar />
      <div className="container loginForm">
        <h3 className="text-center pb-2">
          <b>Login</b>
        </h3>
        {loading ? (
          <div className="text-center">
            <p>Loading...</p>
            <div className="spinner-border" role="status">
              <span className="visually-hidden">Loading...</span>
            </div>
          </div>
        ) : (
          <form onSubmit={handleLogin}>
            <div className="mb-3">
              <label htmlFor="Username" className="form-label">
                <b>Username or Email:</b>
              </label>
              <input
                type="text"
                name="Username"
                value={loginInfo.Username}
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
                value={loginInfo.Password}
                onChange={handleInputChange}
                className="form-control"
                required
              />
            </div>
            <div className="buttonSide">
              <button type="submit" className="btn btn-primary rounded-pill">
                <b>Log In</b>
              </button>
            </div>
          </form>
        )}
        <div>
          <p className="mt-3">
            <Link to="/forgotPassword">
              <b>Forgot Password?</b>
            </Link>
          </p>
          <p className="mt-3">
            Don’t have an account yet?{" "}
            <Link to="/register">
              <b>Sign Up</b>
            </Link>
          </p>
        </div>
      </div>

      {errorModal && (
        <div className="modal" tabIndex="-1" role="dialog">
          <div className="modal-dialog" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  <b>Error</b> <img src={DangerImg} alt="" />
                </h5>

                <button
                  type="button"
                  className="close"
                  data-dismiss="modal"
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

export default Login;