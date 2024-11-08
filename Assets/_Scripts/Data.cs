using System;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
	private DateTime m_startTime;
	private DateTime m_endTime;
	private bool m_isVisible ;
	private bool m_hasAudio;
	private SphericalCoord m_sphericalPos;
	private float m_errorAngle;
	private float m_horErrorAngle;
	private float m_verErrorAngle;

	private static int i = 0;
	private static List<float> Most_recent_N_attempts = new(10);
	private float m_recent_error_average;
    public Data()//
	{
    }
	public Data Start()
	{
		m_startTime = DateTime.UtcNow;
		return this;
	}
	public Data HasAudio(bool val)
	{
		m_hasAudio= val;
		return this;
	}
	public Data IsVisible(bool val)
	{
		m_isVisible = val;
		return this;
	}
	public Data SetPos(SphericalCoord pos)
	{
		m_sphericalPos = pos;
		return this;
	}
	public Data SetErrorAngle(float val)
	{
		if (Most_recent_N_attempts.Count < Most_recent_N_attempts.Capacity) Most_recent_N_attempts.Add(val);
		else Most_recent_N_attempts[i++%Most_recent_N_attempts.Capacity] = val;

		m_recent_error_average =  getAvg();
		m_errorAngle = val;
		return this;
	}
	private float getAvg()
	{
		float sum = 0;
		Most_recent_N_attempts.ForEach(e => sum += e);
		return sum / Most_recent_N_attempts.Count;
	}

	public Data SetHorError(float val)
	{
		m_horErrorAngle = val;
		return this;
	}

    public Data SetVerError(float val)
    {
        m_verErrorAngle = val;
        return this;
    }

    private string m_audioFileName;
	public Data SetAudioFileName(string name)
	{
		m_audioFileName = name;
		return this;
	}


	public Data End()
	{
        m_endTime = DateTime.UtcNow;
		return this;
	}
	public static string COLUMNS = "Start time, End time, Duration(s), isVisible, hasAudio, radius(m), theta(deg), phi(deg), Error Angle (deg), recent_10_attempts_average_error(deg), Horizontal Err Angle, Verticle Err Angle, audioFile";
	override public string ToString()
	{
		if (m_startTime == DateTime.MinValue) throw new Exception("Start time hasn't been set");
		if (m_endTime == DateTime.MinValue) throw new Exception("End time hasn't been set");
        return $"{m_startTime:s}, {m_endTime:s}, {m_endTime.Subtract(m_startTime).TotalSeconds}, {m_isVisible},  {m_hasAudio}, {m_sphericalPos}, {m_errorAngle}, {m_recent_error_average}, {m_horErrorAngle}, {m_verErrorAngle}, {m_audioFileName}";
	}
}
