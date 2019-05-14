using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PageFlipTransition : MonoBehaviour
{
    private BookManager bManager;

    [Header("Rendering")]
    [SerializeField] private Camera bookCamera;

    [SerializeField] private RenderTexture printTexture;
    [SerializeField] private RenderTexture updatedTexture;

    [Header("Scene Elements")]
    [SerializeField] private Animator pageAnimator;
    [SerializeField] private SkinnedMeshRenderer staticPage;
    private Material staticPMat;
    [SerializeField] private SkinnedMeshRenderer fliperPage;
    private Material fliperPMat;

    [Header("Setup")]
    [SerializeField] private float flipTime = 1;

    private void Awake()
    {
        bManager = FindObjectOfType<BookManager>();
        staticPMat = staticPage.material;
        fliperPMat = fliperPage.material;
    }

    public void GoToNextPage(BookPage action)
    {
        StopAllCoroutines();
        StartCoroutine(SwitchGames(true,action));
    }

    public void GoToPrevPage(BookPage action)
    {
        StopAllCoroutines();
        StartCoroutine(SwitchGames(false,action));
    }

    private IEnumerator CameraSetTexture(RenderTexture tex)
    {
        yield return new WaitForEndOfFrame();
        bookCamera.targetTexture = tex;
        yield return new WaitForSeconds(0);
    }

    private void SetupToNextPage()
    {
        fliperPage.material = fliperPMat;
        staticPage.material = staticPMat;
    }

    private void SetupToReturnPage()
    {
        fliperPage.material = staticPMat;
        staticPage.material = fliperPMat;
    }

    private IEnumerator SwitchGames(bool nextGame, BookPage page)
    {
        yield return CameraSetTexture(printTexture);
        yield return CameraSetTexture(updatedTexture);

        pageAnimator.SetFloat("FlipTime", flipTime);

		page.EnablePage();

		if (nextGame)
        {
            SetupToNextPage();
            pageAnimator.SetTrigger("GoNext");
        }
        else
        {
            SetupToReturnPage();
            pageAnimator.SetTrigger("GoPrev");
        }

        yield return new WaitForSeconds(0);
		yield return new WaitForSeconds(flipTime * pageAnimator.GetCurrentAnimatorStateInfo(0).length);

        page.StartPage();
    }

}
