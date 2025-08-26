import { useState } from 'react'
import './App.css'
import CustomerManagement from './components/CustomerManagement'
import BikeManagement from './components/BikeManagement'

function App() {
  const [count, setCount] = useState(0)

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
