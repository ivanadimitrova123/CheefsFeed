import './App.css';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./App.css";
import Register from "./pages/Register";
import LogIn from "./pages/LogIn";
import Feed from './pages/Feed';
import ForgotPassword from "./pages/ForgotPassword";
import AddProfilePicture from "./pages/AddProfilePicture";
import FollowingList from "./pages/FollowingList";
import UserProfile from "./pages/UserProfile";
import SavedRecipes from './pages/SavedRecipes';
import RecipeDetails from './pages/RecipeDetails';
function App() {
  return (
    <Router>
      <ToastContainer position="bottom-center" limit={1} />
      <Routes>
        <Route path="/register" element={<Register/>} />
        <Route path="/login" element={<LogIn/>} />
        <Route path="/forgotPassword" element={<ForgotPassword />} />
        <Route path="/feed" element={<Feed/>} />
        <Route path="/addProfilePicture" element={<AddProfilePicture />} />
        <Route path="/followingList" element={<FollowingList />} />
        <Route path="/userProfile/:id" element={<UserProfile />} />
        <Route path='/savedRecipes' element={<SavedRecipes/>}/>
        <Route path='/recipeDetails/:id' element={<RecipeDetails/>}/>
      </Routes>
    </Router>
  );
}

export default App;
