import './App.css'
import CustomerManagement from './components/CustomerManagement'
import BikeManagement from './components/BikeManagement'

function App() {

  return (
    <div id='container'>
      <div id='right-half'>
        <CustomerManagement/>
      </div>
      <div id='left-half'>
        <BikeManagement/>
      </div>
    </div>
  )
}

export default App
