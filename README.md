# 🟢 AWS Rekognition Setup (Free Tier) + .NET 8 + SQL Server Configuration on Windows

This guide explains how to create a free AWS account, configure Rekognition, install required tools, and prepare your Windows environment to run a .NET 8 project with SQL Server.

---

## ✅ 1. Create a Free AWS Account

1. Visit: [https://aws.amazon.com/free](https://aws.amazon.com/free)
2. Click **“Create a Free Account.”**
3. Fill in your email, password, and account details.
4. Add billing info (credit/debit card, no charge if using Free Tier).
5. Complete phone/SMS verification.
6. Select **Basic Support Plan** (Free).
7. Wait for account activation.

---

## ✅ 2. Enable Amazon Rekognition

1. Log in to the [AWS Console](https://console.aws.amazon.com/)
2. Search for **"Rekognition"** and open the service.
3. Select your region (e.g., `us-east-1`).

📌 Free Tier: 5,000 images/month for 12 months.

---

## ✅ 3. Create an IAM User

1. Go to [IAM Console](https://console.aws.amazon.com/iam)
2. Click **Users > Add Users**
3. Username: `rekognition-user`
4. Enable **Programmatic access**
5. Attach policy: `AmazonRekognitionFullAccess`
6. Create user and save **Access Key ID** and **Secret Access Key**

---

## ✅ 4. Install AWS CLI

1. Download from: [https://aws.amazon.com/cli/](https://aws.amazon.com/cli/)
2. Run the installer with default options.
3. Confirm installation:

```bash
aws --version
```

---

## ✅ 5. Configure AWS CLI

Open Command Prompt and run:

```bash
aws configure
```

Input your credentials:

```
AWS Access Key ID: <your key>
AWS Secret Access Key: <your secret>
Default region: us-east-1
Default output format: json
```

---

## ✅ 6. Install .NET 8 SDK

1. Download from: [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download)
2. Select **.NET 8 SDK (64-bit)** for Windows
3. Run the installer
4. Verify installation:

```bash
dotnet --version
```

5- Install Dotnet tool
```bash
dotnet tool install --global dotnet-ef
```

---

## ✅ 7. Install SQL Server

1. Download **SQL Server Developer Edition**:  
   [https://www.microsoft.com/en-us/sql-server/sql-server-downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Install with default settings
3. Also install **SQL Server Management Studio (SSMS)**:  
   [https://aka.ms/ssmsfullsetup](https://aka.ms/ssmsfullsetup)

---
## ✅ 8. Run the migrations
dotnet ef database update



## ✅ 9. Set Up Your Project

- Ensure your `.NET 8` backend uses the correct connection string for SQL Server.
- Ensure AWS credentials are available for Rekognition calls.
- Use dependency injection for Rekognition clients and SQL DBContext.

