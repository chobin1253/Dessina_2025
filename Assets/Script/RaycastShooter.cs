using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaycastShooter : MonoBehaviour
{
    public ParticleSystem flashEffect;                    //발사 이펙트 변수 선언

    //탄창 관련 변수 선언
    public int magazineCapacity = 30;                     //탄창의 크기
    private int currentAmmo;                              //현재 총알 갯수

    public TextMeshProUGUI ammoUI;                        //총알 개수를 나타낼 TextMeshProUGUI 선언

    public Image reloadingUI; 
    public float reloadTime = 2f;
    private float timer = 0;
    private bool isReloading = false;

    public AudioSource audioSource;
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = magazineCapacity;                      //현재 총알의 갯수를 탄창의 크기만큼으로 변경
        ammoUI.text = $"{currentAmmo}/{magazineCapacity}";
        reloadingUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && isReloading == false)                      //현재 총알이 0개보다 클 떄
        {                                                         //레이 발사 함수 호출
            audioSource.PlayOneShot(audioClip);
            currentAmmo--;                                        //총알을 1개 소비한다.
            flashEffect.Play();                                   //이펙트 재생
            ammoUI.text = $"{currentAmmo}/{magazineCapacity}";       //현재 총알 개수를 UI에 표시 (총알 소비 후 표시!!!)
            ShootRay();
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            isReloading = true;
            reloadingUI.gameObject.SetActive(true);
        }

        if(isReloading == true)
        {
            Reloading();
        }
    }

    void ShootRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);         //발사할 Ray의 시작점, 방향 지정 (메인 카메라에서 마우스 커서 방향으로 발사
        RaycastHit hit;                                                   //Ray를 맞은 대상의 정보를 저장할 저장소

                                                                          //Raycast의 반환형은 bool로, Ray를 맞았다면 true 변환
        if(Physics.Raycast(ray, out hit))
        {                                                                 //맞은 대상 오브젝트를 제거
            Destroy(hit.collider.gameObject);
        }
    }
    void Reloading()
    {
        timer += Time.deltaTime;

        reloadingUI.fillAmount = timer / reloadTime;

        if (timer >= reloadTime)
        {
            timer = 0;
            isReloading = false;                                          //재장전이 완료 됐음을 표시
            currentAmmo = magazineCapacity;                               //총알을 채워준다.
            ammoUI.text = $"{currentAmmo}/{magazineCapacity}";            //현재 총알 개수를 UI에 표시
            reloadingUI.gameObject.SetActive(false);                      //재장전 UI 비활성화
        }
    }
}
