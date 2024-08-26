import './App.css';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "./App.css";
import AddProfilePicture from "./pages/AddProfilePicture";
import ForgotPassword from "./pages/ForgotPassword";
import FollowingList from "./pages/FollowingList";
import RecipeDetails from './pages/RecipeDetails';
import SavedRecipes from './pages/SavedRecipes';
import UserProfile from "./pages/UserProfile";
import RecipeForm from "./pages/RecipeForm";
import Dashboard from './pages/Dashboard';
import Register from "./pages/Register";
import LogIn from "./pages/LogIn";
import Feed from './pages/Feed';

function App() {
  return (
    <Router>
      <ToastContainer position="bottom-center" limit={1} />
      <Routes>
        <Route path="/addProfilePicture" element={<AddProfilePicture />} />
        <Route path='/recipeDetails/:id' element={<RecipeDetails/>}/>
        <Route path="/forgotPassword" element={<ForgotPassword />} />
        <Route path="/followingList" element={<FollowingList />} />
        <Route path="/userProfile/:id" element={<UserProfile />} />
        <Route path="/recipeForm/:id" element={<RecipeForm />} />
        <Route path='/savedRecipes' element={<SavedRecipes/>}/>
        <Route path="/recipeForm" element={<RecipeForm />} />
        <Route path='/admin/dashboard' element={<Dashboard/>}/>
        <Route path="/register" element={<Register/>} />
        <Route path="/login" element={<LogIn/>} />
        <Route path="/feed" element={<Feed/>} />
      </Routes>
    </Router>
  );
}

export default App;
