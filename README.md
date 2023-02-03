# # Pop with Fps

## 💻 프로젝트 소개
키넥트를 활용한 움직임 장려 및 교육 프로그램 제작 

키넥트를 연동해 화면을 보며 사용자가 많이 움직이도록 하는 프로그램입니다.

오브젝트와 사람의 손이 닿으면 지정한 사운드를 내며 점수를 얻을 수 있습니다.



전달 받은 게임의 애저 키넥트 연동, 배경을 투명하게 제거하도록 수정했고 핸드 조인트와 오브젝트가 상호작용 할 수 있도록 콜라이더를 적용하고,  TXT파일 추출과 데이터 정리, 난이도 추가, 전신의 조인트를 이용해 사용자의 움직임을 측정할 수 있도록 했습니다.

손과 오브젝트가 닿으면 오브젝트가 사라지며 그에 해당하는 사운드나 보이스가 출력돼 교육 목적으로 쓰일 수 있도록 구성되어 있습니다.

📌 주요 기능

* 이미지와 사운드를 오브젝트에 삽입해 화면에 떨어트릴 수 있음
* 오브젝트를 손으로 파괴하면 해당하는 소리가 나옴 (ex. A 이미지를 파괴하면 A와 관련되 사운드가 나와 교육의 기능도 함)
* 바디 조인트를 이용해 사람이 얼마나 움직였는지 값을 측정함
* 난이도를 조정해 오브젝트가 나오는 위치의 확률을 변경함 

(1단계는 동일한 확률, 5단계는 사이드에서 나올 확률이 높음 > 몸을 많이 움직여야만 다수의 오브젝트 파괴 가능)



## :hourglass: 개발 기간
2021.08 ~ 2021.10
## 🏃 작업 인원
2명, 
* 하철우 : 프로젝트와 애저키넥트 연동, 바디 조인트에 스크립트 및 콜라이더 삽입, 데이터 추출, 배경 투명화, 난이도 및 게임 매니저 
* 남상훈교수님 : 오브젝트 설정 기본 시스템 및 UI 전체 구현, 키넥트v2 연동

## ⚙️ 개발 환경
* Unity
* Azure Kinect

## :thought_balloon: 초기 구상도

<img src = "https://user-images.githubusercontent.com/84338927/208935871-4605b1d4-dcd2-419e-8b48-8e388f96a51e.PNG" width="70%" height="70%">

## :camera: 실행 화면

<img src = "https://user-images.githubusercontent.com/84338927/208936563-8f092c0f-19ab-4c0b-a14f-5de780441417.PNG" width="50%" height="50%">
<img src = "https://user-images.githubusercontent.com/84338927/208937276-3b35bd0a-3ff7-49f2-9171-f8b01005c4e2.PNG" width="50%" height="50%">
<img src = "https://user-images.githubusercontent.com/84338927/208938258-b64dfd5a-f537-4bad-842b-85eb9e688b21.PNG" width="50%" height="50%">

## :camera: 아두이노 센서 및 미니어처
<img src = "https://user-images.githubusercontent.com/84338927/208937777-7b124720-028f-4da0-9276-c888f23ecbfb.PNG" width="50%" height="50%">
<img src = "https://user-images.githubusercontent.com/84338927/208939039-2d56a0fd-c3b7-40dd-b0a7-8acab4dc9283.PNG" width="50%" height="50%">

## :camera: 전시장
<img src = "https://user-images.githubusercontent.com/84338927/208941514-a167a275-b40a-415a-b0e5-d610f5aa9e68.PNG" width="50%" height="50%">
<img src = "https://user-images.githubusercontent.com/84338927/208945025-6763a081-dd0d-465b-a70a-49028e3e6588.PNG" width="50%" height="50%">




## :camera: CKL 경남콘텐츠코리아랩 전시장 (우수작 외부 출품)
<img src = "https://user-images.githubusercontent.com/84338927/208943931-de969b48-b88d-485b-9ed9-87bf56592126.PNG" width="50%" height="50%">

## 🎥 플레이 영상
[YouTube ⏯️](https://youtu.be/oQ_b3tGW44E)
