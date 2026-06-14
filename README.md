# 🧠 BLD DB API

Backend API for managing and storing algorithms used in **blindfolded Rubik’s Cube solving**.

This API organizes algorithms by case type, validates correctness, prevents duplicates, and provides structured access for training tools and applications like **BLDLAB**.

---

## 🧩 Supported Algorithm Types

- **Edge Commutators** (3-style edge cycles)
- **Corner Commutators** (3-style corner cycles)
- **Parity Algorithms**
  - Standard 2e2c
  - LTCT (Last Two Corners Twist)
  - 2TC (Two Twist Corners)

---

## ⚙️ Current Functionality

### ➕ Insert Algorithms

- Insert a single algorithm tied to a specific case
- Automatically validates:
  - Case correctness
  - Duplicate algorithms
- Invalid or duplicate submissions are rejected

---

### 📦 Bulk Import

- Upload large sets of algorithms (e.g., CSV)
- API will:
  - Detect case for each algorithm
  - Sort valid algorithms into the correct case
  - Discard invalid or unrecognized entries

---

### 📤 Fetch Algorithms

#### Edges & Corners
- Get **all algorithms** (rowewh.com/api/Edges)
- Get algorithms by **buffer** (rowewh.com/api/Edges?buffer=UF)
- Get algorithms for a **specific case** (rowewh.com/api/Edges?buffer=UF&first=UR&second=UL)

#### Parity
- Get **all algorithms** (rowewh.com/api/Parity)
- Get algorithms for a **specific 2E2C case** (rowewh.com/api/Parity?firstEdge=UF&secondEdge=UR&firstCorner=UFR&secondCorner=UBR)
- Get algorithms for a **specific LTCT or T2C case** (rowewh.com/api/Parity?firstEdge=UF&secondEdge=UR&firstCorner=UFR&secondCorner=UBR&twist=LDB)

---

## 🧠 Core Behavior

- Algorithms are **parsed and analyzed** to determine their case
- Cases are **normalized** (handling rotations / variations)
- Data is stored in a **consistent, non-duplicated format**
- All lookups return **clean, validated results**

---

## 🛠️ Tech Stack

- **C#**
- **.NET 8**
- **SQL Server**
- **Dapper** (data access)
  
