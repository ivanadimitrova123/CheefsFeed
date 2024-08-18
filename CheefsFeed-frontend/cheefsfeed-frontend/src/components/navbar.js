import { Link } from "react-router-dom";
import ChefsFeedImage from "../assets/images/ChefsFeed.png";
import "bootstrap/dist/css/bootstrap.min.css";

const Navbar = () => {


  return (
    <nav className="navbar navbar-expand-lg navbar-light sticky-top">
      <div className="container-fluid">
        <Link className="navbar-brand" to="/">
          <img src={ChefsFeedImage} alt="Logo" height="30" />
        </Link>
      
      </div>
    </nav>
  );
};

export default Navbar;